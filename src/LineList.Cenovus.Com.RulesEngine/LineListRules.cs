using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using LineList.Cenovus.Com.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace LineList.Cenovus.Com.RulesEngine
{
    public class RuleResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public RuleResult(bool success, string errorMessage = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
    }

    public class LineListRules
    {
        private readonly ILineListStatusService _lineListStatusService;
        private readonly IEpCompanyAlphaService _epCompanyAlphaService;
        private readonly ILineListRevisionService _lineListRevisionService;
        private readonly ILineRevisionService _lineRevisionService;
        private readonly ILineRevisionSegmentService _segmentService;
        private readonly ILineRevisionOperatingModeService _modeService;
        private readonly IConfiguration _configuration;
        private readonly ILineListStatusStateService _lineListStatusStateService;
        private readonly IEpProjectUserRoleService _epProjectUserRoleService;
        private readonly CurrentUser _currentUser;
        private readonly IUserPreferenceService _userPreferenceService;
        private readonly IConcurrentEngineeringLineService _concurrentEngineeringLineService;
        private readonly ILineService _lineService;

        public LineListRules(
        ILineListStatusService lineListStatusService,
        IEpCompanyAlphaService epCompanyAlphaService,
        ILineListRevisionService lineListRevisionService,
        ILineRevisionService lineRevisionService,
          ILineRevisionSegmentService segmentService,
            ILineRevisionOperatingModeService modeService,
        ILineListStatusStateService lineListStatusStateService,
        IEpProjectUserRoleService epProjectUserRoleService,
             CurrentUser currentUser,
        IConfiguration configuration,
        IUserPreferenceService userPreferenceService,
        IConcurrentEngineeringLineService concurrentEngineeringLineService,
        ILineService lineService
        )
        {
            _lineListStatusService = lineListStatusService;
            _epCompanyAlphaService = epCompanyAlphaService;
            _lineListRevisionService = lineListRevisionService;
            _lineRevisionService = lineRevisionService;
            _segmentService = segmentService;
            _modeService = modeService;
            _lineListStatusStateService = lineListStatusStateService;
            _configuration = configuration;
            _epProjectUserRoleService = epProjectUserRoleService;
            _currentUser = currentUser;
            _userPreferenceService = userPreferenceService;
            _concurrentEngineeringLineService = concurrentEngineeringLineService;
            _lineService = lineService;
        }

        public static bool HasBeenIssued(ref LineListRevision rev)
        {
            return rev.LineList.LineListRevisions.Where(m => m.LineListStatus.IsIssuedOfId != null).Any();
        }

        public string GetNewRevision(Guid facilityId, Guid lineListStatusId, Guid epCompanyId)
        {
            string alpha = GetAlpha(facilityId, epCompanyId);
            var status = _lineListStatusService.GetById(lineListStatusId).Result;
            if (status.IsHardRevision)
                return "0" + alpha;
            else
                return alpha + "A";
        }

        public string GetAlpha(Guid facilityId, Guid? epCompanyId)
        {
            var alpha = string.Empty;
            var alphas = _epCompanyAlphaService.GetAll().Result.Where(m => m.EpCompanyId == epCompanyId && m.FacilityId == facilityId);
            if (alphas.Any())
                alpha = alphas.First().Alpha;
            return alpha;
        }
        public IEnumerable<LineListStatus> GetValidStatusForUpRev(Guid lineListRevisionId)
        {
            var rev = _lineListRevisionService.GetById(lineListRevisionId).Result;
            if (rev == null) return Enumerable.Empty<LineListStatus>();

            var states = _lineListStatusStateService.GetAll().Result
                .Where(s => s.CurrentStatusId == rev.LineListStatusId && s.IsForUpRev);

            return states
                .Select(s => _lineListStatusService.GetById(s.FutureStatusId).Result)
                .Where(s => s != null)
                .OrderBy(s => s.SortOrder);
        }

        public static List<LineListStatus> GetValidStatus(Guid lineListRevisionId, bool isForUpRev, bool includeCurrent)
        {
            List<LineListStatus> list = new List<LineListStatus>();
            using (var db = new LineListDbContext())
            {
                var lineListRev = db.LineListRevisions.FirstOrDefault(m => m.Id == lineListRevisionId) ?? new LineListRevision();

                var states = (from state in db.LineListStatusStates
                              where state.CurrentStatusId == lineListRev.LineListStatusId
                              && state.IsForUpRev == isForUpRev
                              select state).ToList();

                //include the current status
                if (includeCurrent)
                    list.Add(db.LineListStatuses.Single(m => m.Id == lineListRev.LineListStatusId));

                foreach (var state in states)
                {
                    //get states with no requirments
                    if (state.RequiredIssuedStatus1Id == null && state.ExcludeIssuedStatus1Id == null)
                        list.Add(db.LineListStatuses.Single(m => m.Id == state.FutureStatusId));
                    //get states that require a previous status
                    else if (state.RequiredIssuedStatus1Id != null)
                    {
                        if (db.LineListRevisions.Where(m => m.LineListId == lineListRev.LineListId && m.LineListStatusId == state.RequiredIssuedStatus1Id).Any() ||
                        db.LineListRevisions.Where(m => m.LineListId == lineListRev.LineListId && m.LineListStatusId == state.RequiredIssuedStatus2Id).Any() ||
                        db.LineListRevisions.Where(m => m.LineListId == lineListRev.LineListId && m.LineListStatusId == state.RequiredIssuedStatus3Id).Any()
                        )
                            list.Add(db.LineListStatuses.Single(m => m.Id == state.FutureStatusId));
                    }
                    //get states that require a previous state to be excluded
                    else if (state.ExcludeIssuedStatus1Id != null)
                        if (!db.LineListRevisions.Where(m => m.LineListId == lineListRev.LineListId && m.LineListStatusId == state.ExcludeIssuedStatus1Id).Any() &&
                        !db.LineListRevisions.Where(m => m.LineListId == lineListRev.LineListId && m.LineListStatusId == state.ExcludeIssuedStatus2Id).Any() &&
                        !db.LineListRevisions.Where(m => m.LineListId == lineListRev.LineListId && m.LineListStatusId == state.ExcludeIssuedStatus3Id).Any() &&
                            !db.LineListRevisions.Where(m => m.LineListId == lineListRev.LineListId && m.LineListStatusId == state.ExcludeIssuedStatus4Id).Any())
                            list.Add(db.LineListStatuses.Single(m => m.Id == state.FutureStatusId));
                }
            }
            return list.Distinct().OrderBy(m => m.SortOrder).ToList();

        }

        public static List<LineListStatus> GetValidStatusForNew()
        {
            using (var db = new LineListDbContext())
            {
                return db.LineListStatuses.Where(m => m.IsHardRevision == false && m.IsDraftOfId != null).OrderBy(m => m.SortOrder).ToList();
            }
        }

        public static bool ChangeStatus(ref LineListRevision rev, Guid newStatusId, string userName, ILineRevisionService lineRevisionService, ILineListStatusService lineListStatusService, ILineRevisionOperatingModeService lineRevisionOperatingModeService, ILineRevisionSegmentService lineRevisionSegmentService)
        {
            using (LineListDbContext db = new LineListDbContext())
            {
                var updateRevisions = false;
                LineListStatus newStatus = db.LineListStatuses.FirstOrDefault(l => l.Id == newStatusId);

                //if you're changing the status, the revision only changes if you're going hard->soft or soft->hard
                if (rev.LineListStatus.IsHardRevision != newStatus.IsHardRevision)
                {
                    rev.DocumentRevision = GetNextRevision(rev, newStatus);
                    updateRevisions = true;
                }

                if (newStatus.Name.ToUpper().Contains("CANCELLED"))
                {
                    foreach (var line in rev.LineRevisions.Where(m => m.IsReferenceLine == false && m.LineStatus.Name.ToUpper() != "DELETED").ToList())
                    {
                        //reset the line status to "DELETED"
                        line.LineStatusId = db.LineStatuses.Where(x => x.Name.ToUpper() == "DELETED").First().Id;

                        //clear out all engineering data except for SpecificationId, LocationId, CommodityId, and Line Sequence #.
                        LineRules.ClearLineAttributes(line, lineRevisionOperatingModeService, lineRevisionSegmentService);

                        line.ValidationState = (int)LineRevisionHardValidationState.Pass;
                        line.LineRevisionOperatingModes.First().Notes = "DELETED"; // enhancement 133b;
                        line.ModifiedBy = userName;
                        line.ModifiedOn = DateTime.Now;
                    }
                }

                if (!newStatus.Name.ToUpper().Contains("AS BUILT"))
                    foreach (var item in rev.LineRevisions.Where(m => m.IsReferenceLine == false && m.LineStatus.Name.ToUpper() != "DELETED"))
                    {
                        if (item.LineStatusId != newStatus.CorrespondingLineStatusId)
                        {
                            item.LineStatusId = newStatus.CorrespondingLineStatusId;
                            item.Revision = LineRules.GetNextRevision(item, lineRevisionService);
                        }

                        item.LineStatusId = newStatus.CorrespondingLineStatusId;
                    }
                rev.LineListStatusId = newStatusId;
                return updateRevisions;
            }

        }

        public static string GetNextRevision(LineListRevision lineList)
        {
            return GetNextRevision(lineList, lineList.LineListStatus);
        }

        public static string GetNextRevision(LineListRevision rev, LineListStatus newStatus)
        {
            using LineListDbContext Db = new LineListDbContext();
            if (rev == null || newStatus == null) return "(pending)";

            var facilityId = rev.EpProject?.CenovusProject?.FacilityId ?? rev.EpProject?.FacilityId;

            var alpha = Db.EpCompanyAlphas
                .FirstOrDefault(a => a.EpCompanyId == rev.EpCompanyId && a.FacilityId == facilityId)
                ?.Alpha ?? string.Empty;
            if (string.IsNullOrEmpty(alpha)) return "(pending)";

            var curr = rev.DocumentRevision ?? (newStatus.IsHardRevision ? "0" + alpha : alpha + "A");

            if (newStatus.IsHardRevision)
            {
                // parse number up to alpha
                var numPart = curr.Substring(0, curr.Length - alpha.Length);
                if (!int.TryParse(numPart, out var n)) n = 0;
                return $"{n + 1}{alpha}";
            }
            else
            {
                // increment letter at end
                var letter = curr[curr.Length - 1];
                var next = (char)(letter + 1);
                return curr.Substring(0, curr.Length - 1) + next;
            }
        }


        public void UpRevLineList(Guid lineListRevisionId, Guid newStatusId, string nextRevision, string username)
        {
            // 1) Load old revision
            var oldRev = _lineListRevisionService.GetById(lineListRevisionId).Result
                         ?? throw new Exception("Line list revision not found.");

            var newStatus = _lineListStatusService.GetById(newStatusId).Result
                            ?? throw new Exception("Selected status not found.");

            // 2) Create new revision
            var newRev = new LineListRevision
            {
                Id = Guid.NewGuid(),
                LineListId = oldRev.LineListId,
                LineList = oldRev.LineList,
                DocumentRevision = nextRevision,
                DocumentRevisionSort = CalculateRevisionSort(nextRevision),
                LineListStatusId = newStatusId,
                LineListStatus = newStatus,
                EpProjectId = oldRev.EpProjectId,
                EpProject = oldRev.EpProject,
                EpCompanyId = oldRev.EpCompanyId,
                EpCompany = oldRev.EpCompany,
                SpecificationId = oldRev.SpecificationId,
                Specification = oldRev.Specification,
                LocationId = oldRev.LocationId,
                Location = oldRev.Location,
                AreaId = oldRev.AreaId,
                Area = oldRev.Area,
                Description = oldRev.Description,
                PreparedBy = oldRev.PreparedBy,
                ReviewedBy = oldRev.ReviewedBy,
                ApprovedByLead = oldRev.ApprovedByLead,
                ApprovedByProject = oldRev.ApprovedByProject,
                PreparedByProcess = oldRev.PreparedByProcess,
                PreparedByMechanical = oldRev.PreparedByMechanical,
                ReviewByProcess = oldRev.ReviewByProcess,
                ReviewedByMechanical = oldRev.ReviewedByMechanical,
                IsSimpleRevisionBlock = oldRev.IsSimpleRevisionBlock,
                IsActive = true,
                CreatedBy = username,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = username,
                ModifiedOn = DateTime.UtcNow
            };
            _lineListRevisionService.Add(newRev).Wait();

            // 3) Copy each LineRevision plus its Segments & Modes
            var oldLines = _lineRevisionService.GetAll().Result
                              .Where(l => l.LineListRevisionId == lineListRevisionId)
                              .ToList();

            foreach (var oldLine in oldLines)
            {
                // 3a) copy the header
                var newLine = new LineRevision
                {
                    Id = Guid.NewGuid(),
                    LineListRevisionId = newRev.Id,
                    LineListRevision = newRev,
                    LineId = oldLine.LineId,
                    Line = oldLine.Line,
                    IsReferenceLine = oldLine.IsReferenceLine,
                    IsActive = true,
                    CreatedBy = username,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = username,
                    ModifiedOn = DateTime.UtcNow
                };
                _lineRevisionService.Add(newLine).Wait();

                // 3b) copy segments
                var segments = _segmentService.GetAll().Result
                                   .Where(s => s.LineRevisionId == oldLine.Id);
                foreach (var seg in segments)
                {
                    var copy = new LineRevisionSegment
                    {
                        Id = Guid.NewGuid(),
                        LineRevisionId = newLine.Id,
                        SegmentNumber = seg.SegmentNumber,
                        CreatedBy = username,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedBy = username,
                        ModifiedOn = DateTime.UtcNow
                    };
                    _segmentService.Add(copy).Wait();
                }

                // 3c) copy operating modes
                var modes = _modeService.GetAll().Result
                                .Where(m => m.LineRevisionId == oldLine.Id);
                foreach (var md in modes)
                {
                    var copy = new LineRevisionOperatingMode
                    {
                        Id = Guid.NewGuid(),
                        LineRevisionId = newLine.Id,
                        OperatingModeNumber = md.OperatingModeNumber,
                        Notes = md.Notes,
                        CreatedBy = username,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedBy = username,
                        ModifiedOn = DateTime.UtcNow
                    };
                    _modeService.Add(copy).Wait();
                }
            }

            // 4) Deactivate the old revision
            oldRev.IsActive = false;
            oldRev.ModifiedBy = username;
            oldRev.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
            _lineListRevisionService.Update(oldRev).Wait();
        }


        private int CalculateRevisionSort(string revision)
        {
            if (string.IsNullOrEmpty(revision))
            {
                return 0;
            }

            // Assuming revision format is like "0A", "1B", etc.
            var numericPart = int.Parse(revision.Substring(0, revision.Length - 1));
            var letterPart = revision.Substring(revision.Length - 1);
            return numericPart * 100 + (letterPart[0] - 'A' + 1);
        }


        public async Task<RuleResult> RevertToDraft(LineListRevision rev, Guid newRevisionStatus)
        {
            // Fetch all revisions for the same LineList to determine the most recent one
            var revisions = await _lineListRevisionService.GetReservedByLineListId(rev.LineListId);

            var draftRevision = revisions;
            if (draftRevision == null || draftRevision.Id != rev.Id)
            {
                return new RuleResult(false, "Only the most recent revision can be reverted to draft.");
            }


            // Update the revision to the new draft status
            draftRevision.LineListStatusId = newRevisionStatus;
            draftRevision.IsLocked = false;
            draftRevision.LockedBy = string.Empty;
            draftRevision.LockedOn = null;
            draftRevision.IsActive = true;


            // Save changes


            await _lineListRevisionService.Update(draftRevision);
            return new RuleResult(true);
        }

        public static bool ActivePrevRevision(LineListRevision rev, Guid newRevisionStatus)
        {
            using LineListDbContext db = new LineListDbContext();
            var revisions = db.LineListRevisions.Where(m => m.LineListId == rev.LineListId && !m.IsActive).OrderByDescending(m => m.DocumentRevisionSort);
            if (revisions.Any())
            {

                var revisionsFirst = revisions.OrderByDescending(x => x.DocumentRevisionSort).Select(x => revisions.FirstOrDefault());
                revisions.First().IsActive = true;


                foreach (var line in db.LineRevisions.Where(m => m.LineListRevisionId == revisionsFirst.FirstOrDefault().Id))
                {
                    line.IsActive = true;
                }
                db.SaveChanges();

                return true;
            }
            else
                return false;
        }

        public static async Task<bool> Discard(Guid lineListRevisionId)
        {


            using (var lineListDbContext = new LineListDbContext())
            {
                await using var db = new LineListDbContext();
                var llrev = await db.LineListRevisions
                         .Include(r => r.LineListStatus)
                         .FirstOrDefaultAsync(r => r.Id == lineListRevisionId);

                //Only draft line lists can be discarded
                if (llrev == null || llrev.LineListStatus?.IsDraftOfId == null)
                    return false;

                await db.Database.ExecuteSqlRawAsync("DELETE FROM LineRevisionSegment WHERE LineRevisionId IN (SELECT Id FROM LineRevision WHERE LineListRevisionId={0})", lineListRevisionId);
                await db.Database.ExecuteSqlRawAsync("DELETE FROM LineRevisionOperatingMode WHERE LineRevisionId IN (SELECT Id FROM LineRevision WHERE LineListRevisionId={0})", lineListRevisionId);
                await db.Database.ExecuteSqlRawAsync("DELETE FROM LineRevision WHERE LineListRevisionId={0}", lineListRevisionId);
                await db.Database.ExecuteSqlRawAsync("DELETE FROM Line WHERE Id NOT IN (SELECT LineId FROM LineRevision)");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM LineListRevision WHERE Id={0}", lineListRevisionId);
                await db.Database.ExecuteSqlRawAsync("DELETE FROM LineList WHERE ID NOT IN (SELECT LineListId FROM LineListRevision)");
                db.SaveChanges();
                return true;
            }
        }

        public async Task<RuleResult> PrintForIssue(LineListRevision revision, string userName)
        {
            var errors = await ValidForPrintForIssue(revision);

            if (errors.Any())
            {
                throw new Exception("PrintForIssue failed: " + string.Join("; ", errors));
            }

            if (!revision.IsLocked)
            {
                revision.IsLocked = true;
                revision.LockedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
                revision.LockedBy = userName;
                revision.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
                revision.ModifiedBy = userName;
                await _lineListRevisionService.Update(revision);
            }
            return new RuleResult(true);


        }

        public async Task<List<string>> ValidForPrintForIssue(LineListRevision revision)
        {
            var errors = new List<string>();


            if (HasBeenIssued(ref revision))
                errors.Add("This revision has already been issued.");
            if (revision.AreaId == null && string.IsNullOrWhiteSpace(revision.Description))
                errors.Add("Area or Description is required.");
            if (await _lineRevisionService.AnyCheckedOutAsync(revision.Id))
                errors.Add("All lines must be checked in.");

            /// —— NEW: hard - revision validation state check ——
            if (revision.LineRevisions
                .Where(lr => !lr.IsReferenceLine)
                .Any(lr => lr.ValidationState != (int)LineRevisionHardValidationState.Pass)
            && (await _lineListStatusService.GetById(revision.LineListStatusId)).IsHardRevision)
            {
                errors.Add("All lines must pass validation for hard revisions before Print and Issue.");
            }

            if (string.IsNullOrWhiteSpace(revision.LineList?.DocumentNumber))
                errors.Add("Document number must be non-blank.");

            // —— TWEAK: require at least one non-reference line, not just any line ——
            if (!revision.LineRevisions.Any(lr => !lr.IsReferenceLine))
                errors.Add("Line List must have at least one non-reference line.");

            if (revision.IsSimpleRevisionBlock)
            {
                if (string.IsNullOrWhiteSpace(revision.PreparedBy) || string.IsNullOrWhiteSpace(revision.ReviewedBy) ||
                string.IsNullOrWhiteSpace(revision.ApprovedByLead) || string.IsNullOrWhiteSpace(revision.ApprovedByProject))
                    errors.Add("All initials in the simple revision block are required.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(revision.PreparedByMechanical) || string.IsNullOrWhiteSpace(revision.PreparedByProcess) ||
                string.IsNullOrWhiteSpace(revision.ReviewedByMechanical) || string.IsNullOrWhiteSpace(revision.ReviewByProcess) ||
                string.IsNullOrWhiteSpace(revision.ApprovedByLead) || string.IsNullOrWhiteSpace(revision.ApprovedByProject))
                    errors.Add("All initials in the complex revision block are required.");
            }

            return errors;
        }




        public async Task<RuleResult> IssueLineList(LineListRevision revision, DateTime issueDate, string userName, string logoPath = null)
        {
            var errors = await ValidForIssueLineList(revision);
            if (errors.Any())
            {
                return new RuleResult(false, string.Join("; ", errors));


            }

            var currentStatus = await _lineListStatusService.GetById(revision.LineListStatusId);
            var issuedStatus = (await _lineListStatusService.GetAll())
            .FirstOrDefault(s => s.IsIssuedOfId == currentStatus.Id);







            if (issuedStatus == null)
                return new RuleResult(false, "Issued status not found for the current status.");

            revision.LineListStatusId = issuedStatus.Id;
            revision.IsActive = true;
            revision.IssuedOn = issueDate;
            revision.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
            revision.ModifiedBy = userName;

            // Activate this revision and its lines
            var lines = (await _lineRevisionService.GetAll())
            .Where(lr => lr.LineListRevisionId == revision.Id)
            .ToList();
            foreach (var line in lines)
            {
                line.IsActive = true;
                await _lineRevisionService.Update(line);


            }

            // Deactivate prior issued revisions
            var priorRevisions = (await _lineListRevisionService.GetAll())
            .Where(r => r.LineListId == revision.LineListId && r.Id != revision.Id && r.LineListStatus.IsIssuedOfId != null)
            .ToList();
            foreach (var priorRev in priorRevisions)
            {
                priorRev.IsActive = false;
                await _lineListRevisionService.Update(priorRev);
            }

            if (issuedStatus.IsHardRevision)
                NotifyOfIssue(revision, logoPath);

            await _lineListRevisionService.Update(revision);
            return new RuleResult(true);
        }

        public async Task<List<string>> ValidForIssueLineList(LineListRevision revision)
        {
            var errors = await ValidForPrintForIssue(revision);
            if (!revision.IsLocked)
                errors.Add("Line List must be locked.");
            if (revision.EpProjectId == Guid.Empty)
                errors.Add("EP Project is required.");
            if (revision.SpecificationId == Guid.Empty)
                errors.Add("Specification is required.");
            return errors;
        }

        private void NotifyOfIssue(LineListRevision lineListRevision, string logoPath)
        {
            var smtp = new SmtpClient();
            string fromAddress = "line.list@cenovus.com";

            string ccmail = "linelist.linelist@cenovus.com";

            //to = lead engineers for Lead Engineers from EP Project with Lines on Line Lists with Concurrent Engineering
            var toAddresses = GetEmailAddressesDoingConcurrentEngineering(lineListRevision);

            string[] cc = new string[] { ccmail };


            string subject = string.Format("Concurrent Engineering for Line List: {0} {1}", lineListRevision.LineListDocumentNumber, lineListRevision.DocumentRevision);

            string body = "<html><body>";
            body += "Concurrent engineering is taking place on one or more lines included in your project Line List(s).<br />";
            body += "These lines have been recently issued in: <br />";
            body += string.Format("Line List: {0} {1} <br/>", lineListRevision.LineListDocumentNumber, lineListRevision.DocumentRevision);
            body += string.Format("Status: {0} by EP: {1} <br />", lineListRevision.LineListStatus.Name, lineListRevision.EpProject.EpCompany.Description);
            body += "Please refer to the Cenovus Line List Application for the latest concurrent LDT revision and the Cenovus Line List Application for the Concurrent Engineering Report. <br /><br />";
            body += "Line List Application<br /><br />";
            body += "<img src=\"cid:cenovusLogo\" width=\"200\">";
            body += @"</body></html>";

            Regex regex = new Regex("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$");

            foreach (var toAddress in toAddresses)
            {
                if (regex.IsMatch(toAddress))
                {
                    // return;
                    using (var msg = new MailMessage(fromAddress, toAddress) { Subject = subject })
                    {
                        var cenovusLogo = new LinkedResource(logoPath, "image/png");
                        AlternateView altView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                        cenovusLogo.ContentId = "cenovusLogo";
                        altView.LinkedResources.Add(cenovusLogo);
                        msg.AlternateViews.Add(altView);
                        foreach (var email in cc)
                        {
                            if (regex.IsMatch(email))
                            {
                                msg.CC.Add(email);
                            }
                        }
                        smtp.Send(msg);
                    }
                }
            }
        }

        public string[] GetEmailAddressesDoingConcurrentEngineering(LineListRevision lineListRevision)
        {
            //get a list of email addresses for eps performing concurrent engineering on the indicated line list
            //var leadEngRole = ConfigurationManager.AppSettings["RoleEppLeadEng"].ToString();
            var leadEngRole = _configuration.GetValue<string>("ConfigKeys:RoleEppLeadEng");
            //get all epProjects with active line lists with an active line on that matches the loc/comm/seq for the line that was issued
            var lineLists = GetConcurrentEngineeringLineLists(lineListRevision, true, null, null);

            var projects = lineLists.Select(m => m.EpProjectId).ToList();


            var users = from p in _epProjectUserRoleService.GetAll().Result
                        join u in _userPreferenceService.GetAll().Result on p.UserName equals u.UserName into userCache
                        from user in userCache.DefaultIfEmpty()
                        where p.EpProjectRole.Name == leadEngRole
                            && projects.Contains(p.EpProjectId)
                        select (user != null) ? user.Email : p.UserName;


            List<string> usernew = users.ToList();
            //foreach (var user in users)
            //{
            //    CurrentUser usercheck = new CurrentUser();
            //    UserPreference validuser = _userPreferenceService.GetAll().Result.Where(m => m.Email == user).FirstOrDefault();
            //    if (!usercheck.IsActiveADgroups(validuser.UserName))
            //    {
            //        usernew.Remove(user);
            //    }
            //}
            // return users.Distinct().ToArray<string>();
            return usernew.Distinct().ToArray<string>();
        }

        public List<LineListRevision> GetConcurrentEngineeringLineLists(LineListRevision lineListRevision, bool activeOnly, DateTime? IssuedFrom, DateTime? IssuedTo)
        {
            //TODO Get Concurrent Lines
            var lines = _concurrentEngineeringLineService.GetFilteredLines(lineListRevision.Id, lineListRevision.Id, DateTime.Now, DateTime.Now, false).Result;
            var lineLists = lines.Select(m => m.LineListRevisionId).Distinct();
            var matching = from r in _lineListRevisionService.GetAll().Result
                           where lineLists.Contains(r.Id)
                           select r;

            //only grab the 1 of each of the line lists
            return matching.Distinct().ToList();
        }

        public IQueryable<ConcurrentEngineeringLine> GetConcurrentEngineeringLines(Guid? lineListRevisionId, Guid? epProjectId, Guid? facilityId, bool? asBuiltOnly, DateTime? IssuedFrom, DateTime? IssuedTo)
        {
            IQueryable<ConcurrentEngineeringLine> lineRevs;

            if (lineListRevisionId.HasValue)
                lineRevs = (from ce in _concurrentEngineeringLineService.GetAll().Result
                            join line in _lineService.GetAll().Result on ce.LineId equals line.Id
                            join rev in _lineRevisionService.GetAll().Result on line.Id equals rev.LineId
                            where rev.LineListRevisionId == lineListRevisionId && !rev.IsReferenceLine
                            select ce).AsQueryable();
            else
                lineRevs = _concurrentEngineeringLineService.GetAll().Result.AsQueryable();

            if (epProjectId.HasValue)
            {
                var lineIds = _lineRevisionService.GetAll().Result.Where(m => m.LineListRevision.EpProjectId == epProjectId && !m.IsReferenceLine).Select(m => m.LineId);
                lineRevs = lineRevs.Where(m => lineIds.Contains(m.LineId));
            }

            if (facilityId.HasValue)
                lineRevs = lineRevs.Where(m => m.FacilityId == facilityId);

            if (asBuiltOnly.HasValue && asBuiltOnly.Value)
            {
                //lineRevs = lineRevs.Where(m => m.AsBuiltCount > 0);
                lineRevs = lineRevs.Where(m => m.AsBuiltCount > 0 && m.LineStatus.ToLower().Contains("As Built")); //Changes for SER755
            }

            if (IssuedFrom.HasValue && IssuedTo.HasValue)
            {
                //SER755
                DateTime dtmIssuedFrom = Convert.ToDateTime(IssuedFrom.Value.Date.ToString()).Date;
                DateTime dtmIssuedTo = Convert.ToDateTime(IssuedTo.Value.Date.ToString()).Date;

                var lineIds = _lineRevisionService.GetAll().Result.Where(m => m.LineListRevision.IssuedOn > dtmIssuedFrom && m.LineListRevision.IssuedOn < dtmIssuedTo && !m.IsReferenceLine).Select(m => m.LineId);
                lineRevs = lineRevs.Where(m => lineIds.Contains(m.LineId));

            }

            return lineRevs;
        }

        public string[] GetEmailAddressLeadEngineerForLineList(LineListRevision lineListRevision)
        {
            var leadEngRole = _configuration.GetValue<string>("ConfigKeys:RoleEppLeadEng");

            var users = from p in lineListRevision.EpProject.EpProjectUsers
                        join u in _userPreferenceService.GetAll().Result on p.UserName equals u.UserName into userCache
                        from user in userCache.DefaultIfEmpty()
                        where p.EpProjectRole.Name == leadEngRole
                        select (user != null) ? user.Email : p.UserName;

            List<string> usernew = users.ToList();

            //foreach (var user in users)
            //    {
            //        CurrentUser usercheck = new CurrentUser();
            //        UserPreference validuser = db.UserPreferences.Where(m => m.Email == user).FirstOrDefault();
            //        if (!usercheck.IsActiveADgroups(validuser.UserName))
            //        {
            //            users.ToList().Remove(user);
            //            usernew.Remove(user);
            //        }
            //    }
            //return users.ToArray();
            return usernew.ToArray();
        }
    }
}
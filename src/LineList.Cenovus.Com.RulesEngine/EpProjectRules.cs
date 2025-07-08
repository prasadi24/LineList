using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using LineList.Cenovus.Com.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net.Mime;

namespace LineList.Cenovus.Com.RulesEngine
{
    public static class EpProjectRules
    {

        public static async Task<bool> CanChangeFacility(EpProject epProject, ILineRevisionService lineRevisionService)
        {
            // Can change if there are no revisions for this project
            var hasRevisions = await lineRevisionService.HasRevisionsForProject(epProject.Id);
            return !hasRevisions;
        }

        //public static bool CanBeTurnedOver(Guid epProjectId, ILineRevisionService lineRevisionService, ILineListRevisionService lineListRevisionService)
        public static async Task<bool> CanBeTurnedOver(Guid epProjectId, ILineRevisionService lineRevisionService, ILineListRevisionService lineListRevisionService)
        {
            bool result = true;

            // Await the result of GetByEpProjectId to resolve the Task<List<LineRevision>>.
            var lineRevisions = await lineRevisionService.GetByEpProjectId(epProjectId);

            var lines = from r in lineRevisions
                        where r.LineListRevision != null && r.LineListRevision.LineListStatus != null && r.LineStatus != null
                        && r.LineListRevision.EpProjectId == epProjectId
                            && (
                                    (r.LineListRevision.LineListStatus.IsDraftOfId != null)
                                    || (r.LineStatus.IsDefaultForReservation == true)
                            )
                        select r;

            result = !lines.Any();

            return result;
        }

        public static bool HasIssuedLineList(EpProject epProject, ILineListRevisionService lineListRevisionService)
        {
            return lineListRevisionService.GetAll().Result.Where(m => m.LineListStatus.IsIssuedOfId != null && m.EpProjectId == epProject.Id).Any();
        }

        public static async Task<bool> TurnOver(Guid epProjectId,
            Guid epCompanyId,
            CurrentUser performedBy,
            string logoPath,
            IConfiguration configuration
            )
        {
            var result = true;
            using (var db = new LineListDbContext())
            {

                var epProject = await db.EpProjects
                                 .Include(e => e.CenovusProject)
                                 .FirstOrDefaultAsync(e => e.Id == epProjectId);

                var oldEpCompany = db.EpCompanies.Include(e => e.EpCompanyAlphas).Single(m => m.Id == epProject.EpCompanyId);
                var newEpCompany = db.EpCompanies.Include(e => e.EpCompanyAlphas).Single(m => m.Id == epCompanyId);
                var facilityId = epProject.CenovusProject == null ? epProject.FacilityId : epProject.CenovusProject.FacilityId;
                if (newEpCompany.EpCompanyAlphas.Where(m => m.FacilityId == facilityId).Any())
                {
                    epProject.EpCompanyId = epCompanyId;
                    epProject.ModifiedBy = performedBy.Username;
                    epProject.ModifiedOn = DateTime.Now;

                    //6.3.5 PT3 - remove previous ep roles
                    var roles = db.EpProjectUserRoles.Where(m => m.EpProjectId == epProjectId);
                    foreach (var role in roles)
                        db.EpProjectUserRoles.Remove(role);

                    //SER 487
                    var reservedLineLists = db.LineListRevisions.Where(m => m.EpProjectId == epProjectId && m.LineListStatus.Description == "Reserved");
                    foreach (var reservedLineList in reservedLineLists)
                    {
                        reservedLineList.EpCompanyId = epCompanyId;
                        var alphas = newEpCompany.EpCompanyAlphas.Where(m => m.FacilityId == facilityId);
                        string revision = "A";
                        if (alphas.Any())
                            reservedLineList.DocumentRevision = alphas.First().Alpha + revision;

                        reservedLineList.ModifiedBy = performedBy.Username;
                        reservedLineList.ModifiedOn = DateTime.Now;
                    }
                    //db.EpProjectUserRole.Remove(role);

                    db.SaveChanges();

                    var env = configuration.GetValue<string>("Environment");
                    if (env == "PROD" || env == "TQA")
                        NotifyOfTurnOver(epProject, oldEpCompany, newEpCompany, performedBy, logoPath, configuration);
                }
                else
                    result = false;
            }
            return result;
        }

        private static void NotifyOfTurnOver(EpProject epProject, EpCompany oldEpCompany, EpCompany newEpCompany, CurrentUser performedBy,
            string logoPath, IConfiguration configuration)
        {
            var smtp = GetSmtpClient(configuration);

            string fromAddress = configuration.GetValue<string>("EmailSettings:SenderAddress");
            string recipientAddress = configuration.GetValue<string>("EmailSettings:RecipientAddress");
            string ccmail = configuration.GetValue<string>("EmailSettings:CCAddress");

            List<string> list = new List<string>();
            list.Add(recipientAddress);
            String[] toAddresses = list.ToArray();

            string[] cc = new string[] { ccmail };

            string subject = string.Format("EP Project: {0} Turnover", epProject.Name_dash_Description);

            string body = "<html><body>";
            body += string.Format("Ep Project: {0} has been turned over: <br/>", epProject.Name_dash_Description);
            body += string.Format("From EP: {0} <br />", oldEpCompany.Name_dash_Description);
            body += string.Format("To EP: {0} <br />", newEpCompany.Name_dash_Description);
            body += string.Format("By: {0} <br />", performedBy.FullName);
            body += "Please refer to the Cenovus Line List Application for EP Project details and associated Line Lists. <br /><br />";
            body += "Line List Application<br /><br />";
            body += "<img src=\"cid:cenovusLogo\" width=\"200\">";
            body += @"</body></html>";

            foreach (var toAddress in toAddresses)
                using (var msg = new MailMessage(fromAddress, toAddress) { Subject = subject })
                {

                    var cenovusLogo = new System.Net.Mail.LinkedResource(logoPath, "image/png");
                    AlternateView altView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                    cenovusLogo.ContentId = "cenovusLogo";
                    altView.LinkedResources.Add(cenovusLogo);
                    msg.AlternateViews.Add(altView);

                    foreach (var email in cc)
                        msg.CC.Add(email);
                    smtp.Send(msg);
                }
        }

        private static SmtpClient GetSmtpClient(IConfiguration configuration)
        {

            string smtpServer = configuration.GetValue<string>("EmailSettings:SmtpServer");
            int smtpPort = configuration.GetValue<int>("EmailSettings:SmtpPort");
            bool enableSsl = configuration.GetValue<bool>("EmailSettings:EnableSsl");

            // Send the email using SMTP
            SmtpClient smtpClient = new SmtpClient(smtpServer);
            smtpClient.Port = smtpPort;
            smtpClient.EnableSsl = enableSsl;

            return smtpClient;
        }
        private static string[] GetEmailAddressesEpAdmins(EpCompany[] epCompany, IConfiguration configuration, UserManager userManager)
        {
            string groupName = configuration.GetValue<string>(ConfigKey.EpAdmin);
            List<string> addresses = new List<string>();
            foreach (var company in epCompany)
            {
                var adminGroup = string.Format(groupName, company.ActiveDirectoryGroup);
                var users = userManager.GetGroupMembersFromAzureAd(adminGroup).Result;
                foreach (var userName in users)
                    addresses.Add(userManager.UpdateUserCache(userName).Result.Email);
            }
            return addresses.Distinct().ToArray();
        }

        //public static bool Delete(Guid epProjectId, LineListDbContext db)
        //{
        //    //LLDatabase db = new LLDatabase();
        //    //db.Configuration.AutoDetectChangesEnabled = false;
        //    //db.ObjectContext.ExecuteStoreCommand("DELETE FROM LineListRevision WHERE EpProjectId = {0}", epProjectId);
        //    //db.ObjectContext.ExecuteStoreCommand("DELETE FROM LineList WHERE Id NOT IN (SELECT LineListId FROM LineListRevision)");
        //    //db.ObjectContext.ExecuteStoreCommand("DELETE FROM EpProjectInsulationDefaultDetail WHERE Id IN (SELECT D.Id FROM EpProjectInsulationDefaultDetail D JOIN EpProjectInsulationDefaultColumn C ON D.EpProjectInsulationDefaultColumnId = C.Id JOIN EpProjectInsulationDefault I ON C.EpProjectInsulationDefaultId = I.Id WHERE I.EpProjectId = {0})", epProjectId);
        //    //db.ObjectContext.ExecuteStoreCommand("DELETE FROM EpProjectInsulationDefaultColumn WHERE Id IN (SELECT C.Id FROM EpProjectInsulationDefaultColumn C JOIN EpProjectInsulationDefault I ON C.EpProjectInsulationDefaultId = I.Id WHERE I.EpProjectId = {0})", epProjectId);
        //    //db.ObjectContext.ExecuteStoreCommand("DELETE FROM EpProjectInsulationDefaultRow WHERE Id IN (SELECT R.Id FROM EpProjectInsulationDefaultRow R	JOIN EpProjectInsulationDefault I ON R.EpProjectInsulationDefaultId = I.Id WHERE I.EpProjectId = {0})", epProjectId);
        //    //db.ObjectContext.ExecuteStoreCommand("DELETE FROM EpProjectInsulationDefault WHERE EpProjectId = {0}", epProjectId);
        //    //db.ObjectContext.ExecuteStoreCommand("DELETE FROM EpProjectUserRole WHERE EpProjectId = {0}", epProjectId);
        //    //db.ObjectContext.ExecuteStoreCommand("DELETE FROM EpProject WHERE Id = {0}", epProjectId);
        //    //db.SaveChanges();
        //    return true;
        //}
        public static async Task<bool> Delete(
                Guid epProjectId,
                IEpProjectService epProjectService,
                IEpProjectUserRoleService epProjectUserRoleService,
                ILineListRevisionService lineListRevisionService,
                IEpProjectInsulationDefaultDetailService epProjectInsulationDefaultDetailService,
                IEpProjectInsulationDefaultColumnService epProjectInsulationDefaultColumnService,
                IEpProjectInsulationDefaultRowService epProjectInsulationDefaultRowService,
                IEpProjectInsulationDefaultService epProjectInsulationDefaultService,
                ILineListModelService lineListService
            )
        {
            using (var lineListDbContext = new LineListDbContext())
            {
                await lineListDbContext.Database.ExecuteSqlRawAsync("DELETE FROM LineListRevision WHERE EpProjectId = {0}", epProjectId);
                await lineListDbContext.Database.ExecuteSqlRawAsync("DELETE FROM LineList WHERE Id NOT IN (SELECT LineListId FROM LineListRevision)");
                await lineListDbContext.Database.ExecuteSqlRawAsync("DELETE FROM EpProjectInsulationDefaultDetail WHERE Id IN (SELECT D.Id FROM EpProjectInsulationDefaultDetail D JOIN EpProjectInsulationDefaultColumn C ON D.EpProjectInsulationDefaultColumnId = C.Id JOIN EpProjectInsulationDefault I ON C.EpProjectInsulationDefaultId = I.Id WHERE I.EpProjectId = {0})", epProjectId);
                await lineListDbContext.Database.ExecuteSqlRawAsync("DELETE FROM EpProjectInsulationDefaultColumn WHERE Id IN (SELECT C.Id FROM EpProjectInsulationDefaultColumn C JOIN EpProjectInsulationDefault I ON C.EpProjectInsulationDefaultId = I.Id WHERE I.EpProjectId = {0})", epProjectId);
                await lineListDbContext.Database.ExecuteSqlRawAsync("DELETE FROM EpProjectInsulationDefaultRow WHERE Id IN (SELECT R.Id FROM EpProjectInsulationDefaultRow R	JOIN EpProjectInsulationDefault I ON R.EpProjectInsulationDefaultId = I.Id WHERE I.EpProjectId = {0})", epProjectId);
                await lineListDbContext.Database.ExecuteSqlRawAsync("DELETE FROM EpProjectInsulationDefault WHERE EpProjectId = {0}", epProjectId);
                await lineListDbContext.Database.ExecuteSqlRawAsync("DELETE FROM EpProjectUserRole WHERE EpProjectId = {0}", epProjectId);
                await lineListDbContext.Database.ExecuteSqlRawAsync("DELETE FROM EpProject WHERE Id = {0}", epProjectId);
                lineListDbContext.SaveChanges();
                return true;
            }
        }

        public static async Task CreateReservedLineList(EpProject epProject, string userName,
            ILineListStatusService lineListStatusService,
            ICenovusProjectService projectService,
            IEpCompanyAlphaService alphaService,
            ISpecificationService specificationService,
            ILineListModelService lineListModelService,
            ILineListRevisionService lineListRevisionService)
        {
            var docNo = string.Format("Reserved {0}", epProject.Name);
            var status = lineListStatusService.GetAll().Result.Where(m => m.Name.ToUpper() == "RESERVED").First();
            var spec = specificationService.GetAll().Result.Where(m => m.Name == "TR").First();


            //var cenovusProj = projectService.GetAll().Result.Single(m => m.Id == epProject.CenovusProjectId);
            var alphas = alphaService.GetAll().Result.Where(m => m.EpCompanyId == epProject.EpCompanyId && m.FacilityId == epProject.FacilityId);
            string revision = "A";
            if (alphas.Any())
                revision = alphas.First().Alpha + revision;
            var description = string.Format("Reserved Lines for EP Project {0}", epProject.Name);
            var lineList = new LineListModel()
            {
                Id = Guid.NewGuid(),
                CreatedBy = userName,
                CreatedOn = DateTime.Now,
                ModifiedBy = userName,
                ModifiedOn = DateTime.Now,
                DocumentNumber = docNo
            };
            var rev = new LineListRevision()
            {
                Id = Guid.NewGuid(),
                CreatedBy = userName,
                CreatedOn = DateTime.Now,
                ModifiedBy = userName,
                ModifiedOn = DateTime.Now,
                LineListId = lineList.Id,
                SpecificationId = spec.Id,
                EpCompanyId = epProject.EpCompanyId,
                EpProjectId = epProject.Id,
                Description = description,
                LineListStatusId = status.Id,
                IssuedOn = DateTime.Now,
                DocumentRevision = revision,
                IsActive = true
            };
            await lineListModelService.Add(lineList);
            await lineListRevisionService.Add(rev);
        }

        public static void CopyInsulationTableDefaults(EpProject epProject,
            bool autoSave
            )
        {
            using (var db = new LineListDbContext())
            {
                var mapRow = new Dictionary<Guid, Guid>();
                var mapCol = new Dictionary<Guid, Guid>();

                if (epProject.CopyInsulationTableDefaultsEpProjectId == null)
                {
                    foreach (var item in db.InsulationDefaults.Where(m => m.IsActive == true).ToList())
                    {
                        var oldTableId = item.Id;
                        var table = new EpProjectInsulationDefault()
                        {
                            CreatedBy = item.CreatedBy,
                            Description = item.Description,
                            CreatedOn = item.CreatedOn,
                            EpProjectId = epProject.Id,
                            Id = Guid.NewGuid(),
                            InsulationMaterialId = item.InsulationMaterialId,
                            InsulationTypeId = item.InsulationTypeId,
                            IsActive = item.IsActive,
                            LinkToDocument = item.LinkToDocument,
                            ModifiedBy = item.ModifiedBy,
                            ModifiedOn = item.ModifiedOn,
                            Name = item.Name,
                            Notes = item.Notes,
                            SortOrder = item.SortOrder,
                            SpecificationRevision = item.SpecificationRevision,
                            SpecificationRevisionDate = item.SpecificationRevisionDate,
                            TracingTypeId = item.TracingTypeId
                        };

                        db.EpProjectInsulationDefaults.Add(table);

                        foreach (var oldRow in db.InsulationDefaultRows.Where(m => m.InsulationDefaultId == oldTableId).ToList())
                        {
                            var row = new EpProjectInsulationDefaultRow()
                            {
                                EpProjectInsulationDefault = table,
                                CreatedBy = oldRow.CreatedBy,
                                CreatedOn = oldRow.CreatedOn,
                                Id = Guid.NewGuid(),
                                ModifiedBy = oldRow.ModifiedBy,
                                ModifiedOn = oldRow.ModifiedOn,
                                SizeNpsId = oldRow.SizeNpsId
                            };

                            mapRow.Add(oldRow.Id, row.Id);

                            db.EpProjectInsulationDefaultRows.Add(row);
                        }

                        foreach (var oldCol in db.InsulationDefaultColumns.Where(m => m.InsulationDefaultId == item.Id).ToList())
                        {
                            var col = new EpProjectInsulationDefaultColumn()
                            {
                                EpProjectInsulationDefault = table,
                                Id = Guid.NewGuid(),
                                MaxOperatingTemperature = oldCol.MaxOperatingTemperature,
                                MinOperatingTemperature = oldCol.MinOperatingTemperature,
                                CreatedBy = oldCol.CreatedBy,
                                CreatedOn = oldCol.CreatedOn,
                                ModifiedBy = oldCol.ModifiedBy,
                                ModifiedOn = oldCol.ModifiedOn
                            };

                            mapCol.Add(oldCol.Id, col.Id);

                            db.EpProjectInsulationDefaultColumns.Add(col);
                        }

                        foreach (var oldDtl in db.InsulationDefaultDetails
                            .Where(m => m.InsulationDefaultColumn.InsulationDefaultId == item.Id)
                            .ToList())
                        {
                            var dtl = new EpProjectInsulationDefaultDetail()
                            {
                                CreatedBy = oldDtl.CreatedBy,
                                CreatedOn = oldDtl.CreatedOn,
                                Id = Guid.NewGuid(),
                                InsulationThicknessId = oldDtl.InsulationThicknessId,
                                ModifiedBy = oldDtl.ModifiedBy,
                                ModifiedOn = oldDtl.ModifiedOn,
                                TracingDesignNumberOfTracersId = oldDtl.TracingDesignNumberOfTracersId,
                                EpProjectInsulationDefaultColumnId = mapCol[oldDtl.InsulationDefaultColumnId],
                                EpProjectInsulationDefaultRowId = mapRow[oldDtl.InsulationDefaultRowId]
                            };
                            db.EpProjectInsulationDefaultDetails.Add(dtl);
                        }
                    }
                }
                else
                {
                    foreach (var item in db.EpProjectInsulationDefaults
                                 .Where(m => m.EpProjectId == epProject.CopyInsulationTableDefaultsEpProjectId)
                                 .ToList())
                    {
                        var oldId = item.Id;

                        item.Id = Guid.NewGuid();
                        item.EpProjectId = epProject.Id;
                        db.EpProjectInsulationDefaults.Add(item);

                        foreach (var row in db.EpProjectInsulationDefaultRows
                                     .Where(m => m.EpProjectInsulationDefaultId == oldId)
                                     .ToList())
                        {
                            var oldRowId = row.Id;

                            row.Id = Guid.NewGuid();
                            row.EpProjectInsulationDefaultId = item.Id;

                            mapRow.Add(oldRowId, row.Id);

                            db.EpProjectInsulationDefaultRows.Add(row);
                        }

                        foreach (var col in db.EpProjectInsulationDefaultColumns
                                     .Where(m => m.EpProjectInsulationDefaultId == oldId)
                                     .ToList())
                        {
                            var oldColId = col.Id;

                            col.Id = Guid.NewGuid();
                            col.EpProjectInsulationDefaultId = item.Id;

                            mapCol.Add(oldColId, col.Id);

                            db.EpProjectInsulationDefaultColumns.Add(col);
                        }

                        foreach (var dtl in db.EpProjectInsulationDefaultDetails
                                     .Where(m => m.EpProjectInsulationDefaultColumn.EpProjectInsulationDefaultId == oldId)
                                     .ToList())
                        {
                            dtl.Id = Guid.NewGuid();
                            dtl.EpProjectInsulationDefaultColumnId = mapCol[dtl.EpProjectInsulationDefaultColumnId];
                            dtl.EpProjectInsulationDefaultRowId = mapRow[dtl.EpProjectInsulationDefaultRowId];
                            db.EpProjectInsulationDefaultDetails.Add(dtl);
                        }
                    }
                }

                db.SaveChanges();
            }

        }
    }
}
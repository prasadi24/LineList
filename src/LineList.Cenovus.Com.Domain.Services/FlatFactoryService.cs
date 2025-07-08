//using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
//using LineList.Cenovus.Com.Domain.Models;
//using LineList.Cenovus.Com.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace LineList.Cenovus.Com.Domain.Services
//{
//    public class FlatFactoryService : IFlatFactoryService
//    {
//        private readonly ILineRepository _lineRepository;
//        private readonly ILineListRepository _lineListRepository;
//        private readonly ILineRevisionRepository _lineRevisionRepository;
//        private readonly ILineListRevisionRepository _lineListRevisionRepository;

//        public FlatFactoryService(
//            ILineRepository lineRepository,
//            ILineListRepository lineListRepository,
//            ILineRevisionRepository lineRevisionRepository,
//            ILineListRevisionRepository lineListRevisionRepository)
//        {
//            _lineRepository = lineRepository;
//            _lineListRepository = lineListRepository;
//            _lineRevisionRepository = lineRevisionRepository;
//            _lineListRevisionRepository = lineListRevisionRepository;
//        }

//        public IEnumerable<FlatLineList> ToFlatLineLists(IEnumerable<ImportRow> lineListRows)
//        {
//            // Map ImportRow to FlatLineList
//            return lineListRows.Select(row => new FlatLineList
//            {
//                Id = row.Id,
//                LineList = new LineList
//                {
//                    Id = Guid.NewGuid(),
//                    DocumentNumber = row.excelDocumentNumber,
//                    // Map other properties as needed
//                }
//            }).ToList();
//        }

//        public IEnumerable<FlatLine> ToFlatLines(IEnumerable<ImportRow> lineRows)
//        {
//            // Map ImportRow to FlatLine
//            return lineRows.Select(row => new FlatLine
//            {
//                Id = row.Id,
//                Line = new Line
//                {
//                    Id = Guid.NewGuid(),
//                    LineNo = row.excelLineNo,
//                    // Map other properties as needed
//                }
//            }).ToList();
//        }

//        public IEnumerable<LineListRevision> ToLineListRevisions(IEnumerable<FlatLineList> lineLists, string userName)
//        {
//            var lineListRevisions = lineLists.Select(ll => new LineListRevision
//            {
//                Id = Guid.NewGuid(),
//                LineListId = ll.LineList.Id,
//                LineList = ll.LineList,
//                CreatedBy = userName,
//                CreatedOn = DateTime.UtcNow,
//                // Map other properties as needed
//            }).ToList();

//            foreach (var llr in lineListRevisions)
//            {
//                _lineListRepository.Add(llr.LineList);
//                _lineListRevisionRepository.Add(llr);
//            }
//            _lineListRepository.SaveChanges().GetAwaiter().GetResult();
//            _lineListRevisionRepository.SaveChanges().GetAwaiter().GetResult();

//            return lineListRevisions;
//        }

//        public IEnumerable<LineRevision> ToLineRevisions(IEnumerable<FlatLine> lines, string userName, string facility, bool isClone, bool isDraft, Dictionary<string, Guid> lineListIds)
//        {
//            var lineRevisions = lines.Select(l => new LineRevision
//            {
//                Id = Guid.NewGuid(),
//                LineId = l.Line.Id,
//                Line = l.Line,
//                CreatedBy = userName,
//                CreatedOn = DateTime.UtcNow,
//                // Map other properties as needed
//                LineListId = lineListIds.TryGetValue(l.Line.DocumentNumber ?? string.Empty, out var lineListId) ? lineListId : Guid.Empty
//            }).ToList();

//            foreach (var lr in lineRevisions)
//            {
//                _lineRepository.Add(lr.Line);
//                _lineRevisionRepository.Add(lr);
//            }
//            _lineRepository.SaveChanges().GetAwaiter().GetResult();
//            _lineRevisionRepository.SaveChanges().GetAwaiter().GetResult();

//            return lineRevisions;
//        }
//    }
//}
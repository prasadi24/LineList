//using LineList.Cenovus.Com.Domain.Models;
//using System;
//using System.Collections.Generic;

//namespace LineList.Cenovus.Com.Services
//{
//    public interface IFlatFactoryService
//    {
//        IEnumerable<FlatLineList> ToFlatLineLists(IEnumerable<ImportRow> lineListRows);
//        IEnumerable<FlatLine> ToFlatLines(IEnumerable<ImportRow> lineRows);
//        IEnumerable<LineListRevision> ToLineListRevisions(IEnumerable<FlatLineList> lineLists, string userName);
//        IEnumerable<LineRevision> ToLineRevisions(IEnumerable<FlatLine> lines, string userName, string facility, bool isClone, bool isDraft, Dictionary<string, Guid> lineListIds);
//    }
//}
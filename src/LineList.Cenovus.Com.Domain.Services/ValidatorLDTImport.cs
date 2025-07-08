//using LineList.Cenovus.Com.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace LineList.Cenovus.Com.RulesEngine
//{
//    public class ValidatorLDTImport
//    {
//        public IEnumerable<ValidationException> Validate(IEnumerable<FlatLineList> flatLists)
//        {
//            var exceptions = new List<ValidationException>();

//            // Check for required fields
//            foreach (var flatList in flatLists)
//            {
//                if (string.IsNullOrWhiteSpace(flatList.DocumentNumber))
//                {
//                    exceptions.Add(new ValidationException
//                    {
//                        LineList = flatList,
//                        Validation = new ValidationError
//                        {
//                            FieldName = "DocumentNumber",
//                            Message = "Document Number is required"
//                        }
//                    });
//                }

//                if (string.IsNullOrWhiteSpace(flatList.Revision))
//                {
//                    exceptions.Add(new ValidationException
//                    {
//                        LineList = flatList,
//                        Validation = new ValidationError
//                        {
//                            FieldName = "Revision",
//                            Message = "Revision is required"
//                        }
//                    });
//                }
//            }

//            return exceptions;
//        }

//        public IEnumerable<ValidationException> Validate(IEnumerable<FlatLine> flatLines)
//        {
//            var exceptions = new List<ValidationException>();

//            foreach (var flatLine in flatLines)
//            {
//                if (string.IsNullOrWhiteSpace(flatLine.LocationID))
//                {
//                    exceptions.Add(new ValidationException
//                    {
//                        Line = flatLine,
//                        Validation = new ValidationError
//                        {
//                            FieldName = "LocationID",
//                            Message = "Location ID is required"
//                        }
//                    });
//                }

//                if (string.IsNullOrWhiteSpace(flatLine.CommCode))
//                {
//                    exceptions.Add(new ValidationException
//                    {
//                        Line = flatLine,
//                        Validation = new ValidationError
//                        {
//                            FieldName = "CommCode",
//                            Message = "Commodity Code is required"
//                        }
//                    });
//                }

//                if (string.IsNullOrWhiteSpace(flatLine.LineNo))
//                {
//                    exceptions.Add(new ValidationException
//                    {
//                        Line = flatLine,
//                        Validation = new ValidationError
//                        {
//                            FieldName = "LineNo",
//                            Message = "Line Number is required"
//                        }
//                    });
//                }
//            }

//            return exceptions;
//        }
//    }

//    public class ValidationException
//    {
//        public FlatLineList LineList { get; set; }
//        public FlatLine Line { get; set; }
//        public ValidationError Validation { get; set; }
//    }

//    public class ValidationError
//    {
//        public string FieldName { get; set; }
//        public string Message { get; set; }
//    }
//}
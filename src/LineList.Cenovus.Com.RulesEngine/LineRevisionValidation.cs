using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.RulesEngine
{
    public class LineRevisionValidation
    {
        private const string HARDREVISION_VALIDATION_PASS_ICON = "<a href='#' onclick=\"javascript:showValidationErrors('{0}');\"><img src='gfx/linksbtns/pass.png' alt='[Y]' title='Yes' /></a>";
        private const string HARDREVISION_VALIDATION_FAIL_ICON = "<a href='#' onclick=\"javascript:showValidationErrors('{0}');\"><img src='gfx/linksbtns/fail.png' alt='[Y]' title='No' /></a>";
        private const string HARDREVISION_VALIDATION_NOTVALIDATED_ICON = "";

        public static string GetLineHardRevisionIndicator(LineRevision lineRev)
        {
            if (lineRev == null)
                return HARDREVISION_VALIDATION_NOTVALIDATED_ICON;

            var indicator = GetLineHardRevisionIndicator(lineRev.ValidationState);

            return string.Format(indicator, lineRev.Id);
        }

        public static string GetLineHardRevisionIndicator(Guid lineRevisionId, int state)
        {
            if (lineRevisionId == null)
                return HARDREVISION_VALIDATION_NOTVALIDATED_ICON;

            var indicator = GetLineHardRevisionIndicator(state);

            return string.Format(indicator, lineRevisionId);
        }

        public static string GetLineHardRevisionIndicator(int state)
        {
            var validationState = (LineRevisionHardValidationState)state;
            switch (validationState)
            {
                case LineRevisionHardValidationState.Pass:
                    return HARDREVISION_VALIDATION_PASS_ICON;

                case LineRevisionHardValidationState.Fail:
                    return HARDREVISION_VALIDATION_FAIL_ICON;

                case LineRevisionHardValidationState.NotValidated:
                    return HARDREVISION_VALIDATION_NOTVALIDATED_ICON;

                default:
                    return string.Empty;
            }
        }

        public static ActionMessage[] GetHardRevisionValidationErrors(LineRevision rev)
        {
            var list = new List<ActionMessage>();

            //LC2: if line has 'deleted' status, do no validation:
            if (rev.LineStatus != null)
                if (rev.LineStatus.Name.ToUpper() == "DELETED") return list.ToArray();

            //if (rev.LineListRevisionId.HasValue)
            //{
            //    var validator = new Validator(rev.LineListRevisionId.Value, rev.RequiresMinimumInformation);
            //    var lines = FlatFactory.ToFlatLines(rev);
            //    var errors = validator.Validate(lines).ToList();

            //    foreach (var item in errors)
            //        list.Add(new ActionMessage() { Title = item.Validation.FieldName, Message = item.Validation.Message });
            //}
            return list.ToArray();
        }

        public static LineRevisionHardValidationState GetHardRevisionValidationState(LineRevision lineRev)
        {
            if (lineRev == null)
                return LineRevisionHardValidationState.Fail;

            var list = GetHardRevisionValidationErrors(lineRev);

            if (list.Any())
                return LineRevisionHardValidationState.Fail;
            else

                return LineRevisionHardValidationState.Pass;
        }
    }
}
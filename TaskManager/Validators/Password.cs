using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using TaskManager.Extentions;
using TaskManager.ExtentionsMethods;


namespace TaskManager.Validators
{

    public class Password : Attribute, IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            if (!context.Model.ToString().Any<Char>(c => c.IsLetter()))
                return new List<ModelValidationResult> { new ModelValidationResult("", "password must contain a letter") };

            if (!context.Model.ToString().Any<Char>(c => c.IsUpper()))
                return new List<ModelValidationResult> { new ModelValidationResult("", "password must contain a upper case letter") };

            if (!context.Model.ToString().Any<Char>(c => c.IsLower()))
                return new List<ModelValidationResult> { new ModelValidationResult("", "password must contain a lower case letter") };

            if (!context.Model.ToString().Any<Char>(c => c.IsDigit()))
                return new List<ModelValidationResult> { new ModelValidationResult("", "password must contain a digit") };

            if (!context.Model.ToString().Any<Char>(c => c.IsSpecial()))
                return new List<ModelValidationResult> { new ModelValidationResult("", "password must contain a Special Character") };

            return Enumerable.Empty<ModelValidationResult>();


        }


    }
}

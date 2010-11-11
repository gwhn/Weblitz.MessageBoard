using System;
using System.Web.Mvc;
using StructureMap;
using Weblitz.MessageBoard.Core.Domain.Repositories;

namespace Weblitz.MessageBoard.Web.Models.Binders
{
    public class EntityModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (value == null) return null;

            if (string.IsNullOrEmpty(value.AttemptedValue)) return null;

            Guid id;

            if (!Guid.TryParse(value.AttemptedValue, out id)) return null;

            var type = typeof (IKeyedRepository<,>).MakeGenericType(bindingContext.ModelType, typeof (Guid));

            var repository = ObjectFactory.GetInstance(type) as IKeyedRepository<Guid>;

            if (repository == null) return null;

            var entity = repository.FindBy(id);

            return entity;
        }
    }
}
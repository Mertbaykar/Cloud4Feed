using Cloud4Feed.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloud4Feed.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Cloud4Feed.Application.Repository
{
    public abstract class RepositoryBase
    {
        protected ECommerceContext MasterDataDb;

        protected RepositoryBase(ECommerceContext eCommerceContext)
        {
            this.MasterDataDb = eCommerceContext;
        }

        /// <summary>
        /// If invalid, throws exception along with error messages in it. Otherwise execution continues
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="entity"></param>
        /// <param name="validator"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual async Task Validate<TModel>(TModel entity, IValidator<TModel> validator) where TModel : class
        {
            FluentValidation.Results.ValidationResult validationResult = await validator.ValidateAsync(entity);

            if (!validationResult.IsValid)
            {
                StringBuilder stringBuilder = new StringBuilder();
                validationResult.Errors.ForEach(e => stringBuilder.AppendLine(e.ErrorMessage));
                throw new Exception(stringBuilder.ToString());
            }
        }

        #region EF Core

        protected virtual async Task<TEntity> Add<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            MasterDataDb.Set<TEntity>().Add(entity);
            await Save();
            return entity;
        }

        protected virtual async Task<TEntity?> Get<TEntity>(Guid id) where TEntity : EntityBase
        {
            return await MasterDataDb.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }

        protected virtual async Task Delete<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            entity.DeActivate();
            await Save();
        }

        protected virtual async Task Delete<TEntity>(Guid id) where TEntity : EntityBase
        {
            var entity = await MasterDataDb.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new Exception($"{typeof(TEntity).Name} bulunamadı");
            await Delete(entity);
        }

        protected async Task Save()
        {
            await MasterDataDb.SaveChangesAsync();
        } 
        #endregion
    }
}

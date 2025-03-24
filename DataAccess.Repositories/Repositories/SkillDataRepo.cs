
using AutoMapper;
using DataAccess.Entities.Context;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using SkillSheetAPI.Models.DTOs;

namespace DataAccess.Repositories.Repositories
{
    public class SkillDataRepo : ISkillDataRepo
    {
        ApplicationDbContext _skillsheetContext;
        private readonly IMapper _mapper;

        public SkillDataRepo(ApplicationDbContext skillsheetContext, IMapper mapper)
        {
            _skillsheetContext = skillsheetContext;
            _mapper = mapper;
        }

        #region GetSkillCategoryAsync
        /// <summary>
        /// Gets the skill categories asynchronously.
        /// </summary>
        /// <returns>A list of skill categories.</returns>
        public async Task<List<SkillCategoryDTO>> GetSkillCategory()
        {
            var allSkillCategory = await _skillsheetContext.SkillCategories.ToListAsync();
            return _mapper.Map<List<SkillCategoryDTO>>(allSkillCategory);
        }
        #endregion

        #region GetSkillSubcategoryAsync
        /// <summary>
        /// Gets the skill subcategories asynchronously.
        /// </summary>
        /// <returns>A list of skill subcategories.</returns>
        public async Task<List<SkillSubcategoryDTO>> GetSkillSubcategory()
        {
            var allSkillSubcategory = await _skillsheetContext.SkillSubcategories.ToListAsync();
            return _mapper.Map<List<SkillSubcategoryDTO>>(allSkillSubcategory);
        }
        #endregion

        #region GetSkillSubcategoryAsyncById
        /// <summary>
        /// Gets the skill subcategory by category ID asynchronously.
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <returns>A list of skill subcategories.</returns>
        public async Task<List<SkillSubcategoryDTO>> GetSkillSubcategory(int categoryID)
        {
            var skillSubcategory = await _skillsheetContext.SkillSubcategories
                .Where(o => o.CategoryId == categoryID).ToListAsync();
            return _mapper.Map<List<SkillSubcategoryDTO>>(skillSubcategory);
        }
        #endregion

        #region GetSkillAsync
        /// <summary>
        /// Gets the skills asynchronously.
        /// </summary>
        /// <returns>A list of skills.</returns>
        public async Task<List<SkillDTO>> GetSkill()
        {
            var allSkill = await _skillsheetContext.Skills.ToListAsync();
            var orders = await _skillsheetContext.Skills
                .Where(o => o.SkillId == 120)
                .Select(o => new
                {
                    o.SkillName,
                    category = o.Subcategory.Category.CategoryName,
                    name = o.Subcategory.SubcategoryName
                })
                .ToListAsync();

            return _mapper.Map<List<SkillDTO>>(allSkill);
        }
        #endregion

        #region GetSkillAsyncById
        /// <summary>
        /// Gets the skills by subcategory ID asynchronously.
        /// </summary>
        /// <param name="subCategoryID">The subcategory ID.</param>
        /// <returns>A list of skills.</returns>
        public async Task<List<SkillDTO>> GetSkill(int subCategoryID)
        {
            var allSkill = await _skillsheetContext.Skills
                .Where(o => o.SubcategoryId == subCategoryID).ToListAsync();
            return _mapper.Map<List<SkillDTO>>(allSkill);
        }
        #endregion
    }


}


using AutoMapper;
using DataAccess.Repositories.Interfaces;
using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces;

namespace SkillSheetAPI.Services.Services
{
    public class SkillDataService : ISkillDataService
    {
        ISkillDataRepo _skillDataRepo { get; set; }
        private readonly IMapper _mapper;

        public SkillDataService(IMapper mapper, ISkillDataRepo skillDataRepo)
        {
            this._skillDataRepo = skillDataRepo;
            this._mapper = mapper;
        }
        #region GetSkillCategoryServiceAsync
        /// <summary>
        /// Gets the skill categories asynchronously.
        /// </summary>
        /// <returns>A list of skill categories.</returns>
        public async Task<List<SkillCategoryDTO>> GetSkillCategoryService()
        {
            try
            {
                var skillCategory = await _skillDataRepo.GetSkillCategory();
                return _mapper.Map<List<SkillCategoryDTO>>(skillCategory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetSkillSubcategoryServiceAsync
        /// <summary>
        /// Gets the skill subcategories asynchronously.
        /// </summary>
        /// <returns>A list of skill subcategories.</returns>
        public async Task<List<SkillSubcategoryDTO>> GetSkillSubcategoryService()
        {
            try
            {
                var skillSubcategory = await _skillDataRepo.GetSkillSubcategory();
                return _mapper.Map<List<SkillSubcategoryDTO>>(skillSubcategory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetSkillSubcategoryServiceAsyncById
        /// <summary>
        /// Gets the skill subcategory by category ID asynchronously.
        /// </summary>
        /// <param name="categoryID">The category ID.</param>
        /// <returns>A list of skill subcategories.</returns>
        public async Task<List<SkillSubcategoryDTO>> GetSkillSubcategoryService(int categoryID)
        {
            try
            {
                var skillSubcategory = await _skillDataRepo.GetSkillSubcategory(categoryID);
                return _mapper.Map<List<SkillSubcategoryDTO>>(skillSubcategory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetSkillServiceAsync
        /// <summary>
        /// Gets the skills asynchronously.
        /// </summary>
        /// <returns>A list of skills.</returns>
        public async Task<List<SkillDTO>> GetSkillService()
        {
            try
            {
                var skills = await _skillDataRepo.GetSkill();
                return _mapper.Map<List<SkillDTO>>(skills);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetSkillServiceAsyncById
        /// <summary>
        /// Gets the skills by subcategory ID asynchronously.
        /// </summary>
        /// <param name="subCategoryID">The subcategory ID.</param>
        /// <returns>A list of skills.</returns>
        public async Task<List<SkillDTO>> GetSkillService(int subCategoryID)
        {
            try
            {
                var skills = await _skillDataRepo.GetSkill(subCategoryID);
                return _mapper.Map<List<SkillDTO>>(skills);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}


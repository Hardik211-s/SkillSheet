using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Services.Interfaces;

namespace SkillSheetAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillDataController : Controller
    {

        ISkillDataService skillDataService;
        public SkillDataController(ISkillDataService skillDataService)
        {
            this.skillDataService = skillDataService;
        }
        #region GetSkillCategory
        /// <summary>
        /// Gets the skill categories.
        /// </summary>
        /// <returns>A list of skill categories.</returns>
        [HttpGet("Category")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> GetSkillCategory()
        {
            try
            {
                var category = await skillDataService.GetSkillCategoryService();
                return Ok(new { category });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetSkillSubcategory
        /// <summary>
        /// Gets the skill subcategories.
        /// </summary>
        /// <returns>A list of skill subcategories.</returns>
        [HttpGet("Subcategory")]
        public async Task<IActionResult> GetSkillSubcategory()
        {
            try
            {
                var subCategory = await skillDataService.GetSkillSubcategoryService();
                return Ok(new { subCategory });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetSkillSubcategoryById
        /// <summary>
        /// Gets the skill subcategory by ID.
        /// </summary>
        /// <param name="id">The subcategory ID.</param>
        /// <returns>The skill subcategory.</returns>
        [HttpGet("Subcategory/{id}")]
        public async Task<IActionResult> GetSkillSubcategory(int id)
        {
            try
            {
                var subCategory = await skillDataService.GetSkillSubcategoryService(id);
                return Ok(new { subCategory });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetSkill
        /// <summary>
        /// Gets the skills.
        /// </summary>
        /// <returns>A list of skills.</returns>
        [HttpGet("Skill")]
        public async Task<IActionResult> GetSkill()
        {
            try
            {
                var skills = await skillDataService.GetSkillService();
                return Ok(new { skills });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetSkillById
        /// <summary>
        /// Gets the skill by ID.
        /// </summary>
        /// <param name="id">The skill ID.</param>
        /// <returns>The skill.</returns>
        [HttpGet("Skill/{id}")]
        public async Task<IActionResult> GetSkill(int id)
        {
            try
            {
                var skills = await skillDataService.GetSkillService(id);
                return Ok(new { skills });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }

}


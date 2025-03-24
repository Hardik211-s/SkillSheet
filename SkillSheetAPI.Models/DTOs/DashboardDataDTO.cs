using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkillSheetAPI.Models.DTOs
{
    public class DashboardDataDTO
    {

        public int TotalUser {  get; set; }

        public int TotalSkill { get; set; }

        public List<UserAllDataDTO> UserAllData { get; set; }

        public double ExperienceAVG { get; set; }

        public List<DbUserDetailDTO> AllUserDetail { get; set; }

    }
}

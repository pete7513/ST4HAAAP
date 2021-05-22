using System;

namespace DTO
{

    public class DTO_StaffLogin
    {
        public string Name { get; set; }

        public int StaffID { get; set; }

        public string Password { get; set; }

        public StaffStatus StaffStatus { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels
{
    public class DashBoardAdminViewModel
    {
        public int TotalPropertiesAvailable { get; set; } 
        public int TotalPropertiesSold { get; set; }      

        public int TotalAgentsActive { get; set; }        
        public int TotalAgentsInactive { get; set; }      

        public int TotalClientsActive { get; set; }       
        public int TotalClientsInactive { get; set; }     

        public int TotalDevelopersActive { get; set; }    
        public int TotalDevelopersInactive { get; set; }

    }
}

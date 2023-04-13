using PersonalFinanceMVC.Models.Entities;

namespace PersonalFinanceMVC.Models
{
    public class DataService
    {
        private readonly ApplicationContext context;
        public DataService(ApplicationContext context)
        {
            this.context = context;
        }
    }
}

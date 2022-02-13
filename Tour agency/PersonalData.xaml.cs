using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tour_agency.Model.Base;

namespace Tour_agency
{
    /// <summary>
    /// Логика взаимодействия для PersonalData.xaml
    /// </summary>
    public partial class PersonalData : Window
    {
        Constants.VisitorType TypeOfVisitor { get; }
        int VisitorId { get; }
        public PersonalData(Constants.VisitorType visitorType, int visitorId)
        {
            InitializeComponent();
            TypeOfVisitor = visitorType;
            VisitorId = visitorId;
            
            Man visitor;

            using (var agencyDbContext = new AgencyDbContext()) {
                if (TypeOfVisitor == Constants.VisitorType.Client)
                {
                    visitor = agencyDbContext.Clients.Find(VisitorId);
                }
                else
                {
                    visitor = agencyDbContext.Managers.Find(VisitorId);
                }
            }

            tbId.Text = visitor.Id.ToString();
            tbName.Text = visitor.Name;
            tbPhone.Text = visitor.Phone;

        }
    }
}

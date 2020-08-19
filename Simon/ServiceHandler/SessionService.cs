using System;
using System.Collections.ObjectModel;
using Simon.Models;

namespace Simon.ServiceHandler
{
    public class SessionService
    {
        public static ObservableCollection<FooterModel> BaseFooterItems { get; set; }
        public static FooterModel SelectedFooterItem { get; set; }
    }
}

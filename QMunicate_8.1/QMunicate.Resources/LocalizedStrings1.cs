
namespace QMunicate.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LocalizedStrings : BindableObject
    {


		private static QMunicate.Resources.ApplicationResources.ApplicationResources applicationResources = new QMunicate.Resources.ApplicationResources.ApplicationResources();

		public static QMunicate.Resources.ApplicationResources.ApplicationResources ApplicationResources 
		{
			get
			{
				return applicationResources;
			}
		}
	
		public void ChangeLanguage()
		{
			
			RaisePropertyChanged("ApplicationResources");
			
		}
	}
}

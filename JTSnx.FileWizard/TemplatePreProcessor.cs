using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TemplateWizard;

namespace JTSnx.FileWizard
{
    public class TemplatePreProcessor : IWizard
    {
        #region Constants

        private const string SERVICES_FOLDER = "Services";
        private const string REPOSITORIES_FOLDER = "Repositories";
        private const string DOMAIN_FOLDER = "Domain";

        private const string REPOSITORY_NAME = "Repository";
        private const string SERVICE_NAME = "Service";
        private const string DOMAIN_NAME = "Domain";

        #endregion

        #region Fields

        private readonly DTE _dte;
        private IEnumerable<Project> _allProjects;
        private ProjectItem _tempFolder;

        private string _className;

        private string _startingNamespace;

        private string _DAL;
        private string _shortNamespace;
        private string _DALTest;

        private string _BLL;
        private string _BLLNamespace;
        private string _BLLTest;

        private string _API;
        private string _apiNamespace;
        
        #endregion

        #region Ctor

        public TemplatePreProcessor()
        {
            _dte = (DTE)Package.GetGlobalService(typeof(DTE));
        }

        #endregion

        #region IWizard Methods

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {

            string itemName = Path.GetFileNameWithoutExtension(projectItem.Name);
            string rootProject = projectItem.ContainingProject.Name;




            var newFolder = _startingNamespace.Substring(rootProject.Length + 6).Replace(".","\\");
            

         //   string newFolder = string.Empty;
            string folder = string.Empty;
            
           
            Project project = null;

            if (itemName == _className)
            {
                project = GetProjectByName(_DAL);
                folder = "Repo\\"+ newFolder;
            }
            else if (itemName == "I" + _className)
            {
                project = GetProjectByName(_BLL);
                folder = "Interfaces\\" + newFolder;
            }
            else if (itemName == _className + "Converter")
            {
                project = GetProjectByName(_BLL);
                folder = "Converters\\" + newFolder;
            }
            else if (itemName == "LS" + _className)
            {
                project = GetProjectByName(_BLL);
                folder = "LookupService\\" + newFolder;
            }
            else if (itemName == _className + "Controller")
            {
                project = GetProjectByName(_API);
                folder = "Controllers\\" + newFolder;
            }
           

            project.AddToFolder(projectItem, folder);
            _tempFolder = (ProjectItem)projectItem.Collection.Parent;
        }

        public void RunStarted(
            object automationObject,
            Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind,
            object[] customParams)
        {
            _allProjects = _dte.Solution.GetProjects();

            OptionsForm form = new OptionsForm(_allProjects.Select(it => it.Name));
            form.ShowDialog();

            _DAL = form.SelectedDALProject;
            _BLL = form.SelectedBLLProject;
            _API = form.SelectedAPIProject;

            _startingNamespace = replacementsDictionary["$rootnamespace$"];
            _shortNamespace = _startingNamespace.Replace(_DAL + ".Repo.", "");
            _className = replacementsDictionary["$safeitemname$"];

            // Format the model name in case when someone typed 'eMploYeE' instead of 'Employee'
            //_className = CultureInfo.CurrentCulture.TextInfo.(_className);

            SetParameters(replacementsDictionary);
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public void BeforeOpeningFile(ProjectItem projectItem) { }

        public void ProjectFinishedGenerating(Project project) { }

        public void RunFinished()
        {
            _tempFolder?.Remove();
            _tempFolder?.Delete();
        }

        #endregion

        #region Private Methods       

        private void SetParameters(Dictionary<string, string> replacementsDictionary)
        {
            var facade = _shortNamespace.Split(new Char[]{'.'});
           
            replacementsDictionary.Add("$basename$", _className);
            replacementsDictionary.Add("$interfaceNamespace$", string.Concat(_BLL, ".Interfaces.", _shortNamespace));
            replacementsDictionary.Add("$converterNamespace$", string.Concat(_BLL, ".Converters.", _shortNamespace));
            replacementsDictionary.Add("$serviceNamespace$", string.Concat(_BLL, ".LookupService.", _shortNamespace));
            replacementsDictionary.Add("$controllerNamespace$", string.Concat(_API, ".Controllers.", _shortNamespace ));
            replacementsDictionary.Add("$facadeName$", string.Concat(facade[0], "Facade"));
            replacementsDictionary.Add("$apiComment$", string.Concat("// GET: api/", _className, "/**Enter the name of the call here**"));

        }

        private Project GetProjectByName(string projectName)
        {
            return _allProjects.FirstOrDefault(it => it.Name == projectName);
        }

       


        #endregion
    }
}
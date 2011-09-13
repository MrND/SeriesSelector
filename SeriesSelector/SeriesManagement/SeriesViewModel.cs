using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Input;
using FolderPickerLib;
using SeriesSelector.Data;
using SeriesSelector.Frame;
using SeriesSelector.Properties;

namespace SeriesSelector.SeriesManagement
{
    [Export("Series", typeof(object))]
    public class SeriesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;



        public SeriesViewModel()
        {

            _sourcePath = Settings.Default.SourcePath;
            _destinationPath = Settings.Default.DestinationPath;
            _fileList = new ObservableCollection<EpisodeType>();
            _newFileList = new ObservableCollection<EpisodeType>();
            _currentMappings = new Dictionary<string, string>();
            SelectFolder = new AdHocCommand(ExecuteSelectFolder);
            SelectDestinationFolder = new AdHocCommand(ExecuteSelectDestinationFolder);
            AddMapping = new AdHocCommand(ExecuteAddMapping);
            RemoveMapping = new AdHocCommand(ExecuteRemoveMapping);
            MoveAllFiles = new AdHocCommand(ExecuteMoveAllFiles);
            MoveSelectedFile = new AdHocCommand(ExecuteMoveSelectedFile);
            _episodeService = BootStrapper.Resolve<IEpisdoeService>();
            FileTypes = new ObservableCollection<FileTypeValue>
                             {
                                 new FileTypeValue("*.avi"),
                                 new FileTypeValue("*.mkv")
                             };
            _selectedFileType = new FileTypeValue("*avi");
        }

        private FileTypeValue _selectedFileType;
        public FileTypeValue SelectedFileType
        {
            get { return _selectedFileType; }
            set
            {
                _selectedFileType = value;
                PropertyChanged(this, new PropertyChangedEventArgs("FileList"));
                PropertyChanged(this, new PropertyChangedEventArgs("NewFileList"));
            }
        }

        public ObservableCollection<FileTypeValue> FileTypes { get; set; }

        private void ExecuteRemoveMapping(object obj)
        {
            _currentMappings.Remove(_selectedMapping.Key);
            _episodeService.WriteMappingValue(_currentMappings);
            PropertyChanged(this, new PropertyChangedEventArgs("CurrentMappings"));
        }

        public ICommand RemoveMapping { get; set; }

        private KeyValuePair<string, string> _selectedMapping;

        public KeyValuePair<string, string> SelectedMapping
        {
            get { return _selectedMapping; }
            set
            {
                _selectedMapping = value;
                _newName = _selectedMapping.Value;
                PropertyChanged(this, new PropertyChangedEventArgs("NewName"));
            }
        }

        private void ExecuteAddMapping(object obj)
        {
            _selectedFile = null;
            CurrentMappings.Add(Name, NewName);
            _episodeService.WriteMappingValue(_currentMappings);
            PropertyChanged(this, new PropertyChangedEventArgs("CurrentMappings"));
            PropertyChanged(this, new PropertyChangedEventArgs("NewFileList"));
        }

        private string _newName;

        public string NewName
        {
            get { return _newName; }
            set { _newName = value; }
        }

        private readonly IEpisdoeService _episodeService;

        private Dictionary<string, string > _currentMappings;

        public ICommand AddMapping { get; set; }

        private string _sourcePath;

        public string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                _sourcePath = value;
               
            }
        }

        private string _destinationPath;

        public string DestinationPath
        {
            get { return _destinationPath; }
            set { _destinationPath = value; }
        }

        public ICommand SelectFolder { get; private set; }

        public void ExecuteSelectFolder(object obj)
        {
            var dlg = new FolderPickerDialog();
            if (dlg.ShowDialog() == true)
            {
                _sourcePath = dlg.SelectedPath;
                PropertyChanged(this, new PropertyChangedEventArgs("SourcePath"));
                PropertyChanged(this, new PropertyChangedEventArgs("FileList"));
                PropertyChanged(this, new PropertyChangedEventArgs("NewFileList"));
            }
        }

        public ICommand SelectDestinationFolder { get; set; }

        private void ExecuteSelectDestinationFolder(object obj)
        {
            var dlg = new FolderPickerDialog();
            if (dlg.ShowDialog() == true)
            {
                _destinationPath = dlg.SelectedPath;
                PropertyChanged(this, new PropertyChangedEventArgs("DestinationPath"));
            }
        }

        public ICommand MoveAllFiles { get; set; }
        private void ExecuteMoveAllFiles(object obj)
        {
            foreach (var episodeType in _newFileList)
            {
                string oldPath = episodeType.FullPath;
                string newName;
                _currentMappings.TryGetValue(episodeType.FileName, out newName);
                string newPath = _destinationPath + "\\" + newName + "\\" + episodeType.Season.ToUpper();

                
                    if (Directory.Exists(newPath))
                    {
                        newPath = newPath + "\\" + episodeType.NewName + "." + episodeType.FileType;
                        if(!File.Exists(newPath))
                            File.Move(oldPath, newPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(newPath);
                        newPath = newPath + "\\" + episodeType.NewName + "." + episodeType.FileType;
                        File.Move(oldPath, newPath);
                    }
            }
        }

        public ICommand MoveSelectedFile { get; set; }
        public void ExecuteMoveSelectedFile(object obj)
        {
            var episodeType = _selectedFile;
            string oldPath = episodeType.FullPath;
            string newName;
            _currentMappings.TryGetValue(episodeType.FileName, out newName);
            string newPath = _destinationPath + "\\" + newName + "\\" + episodeType.Season.ToUpper();

                if (Directory.Exists(newPath))
                {
                    newPath = newPath + "\\" + episodeType.NewName + "." + episodeType.FileType;
                    File.Move(oldPath, newPath);
                }
                else
                {
                    Directory.CreateDirectory(newPath);
                    newPath = newPath + "\\" + episodeType.NewName + "." + episodeType.FileType;
                    File.Move(oldPath, newPath);
                }


        }

        private readonly ObservableCollection<EpisodeType> _newFileList;

        public ObservableCollection<EpisodeType> NewFileList
        {
            get
            {
                _newFileList.Clear();
                
                foreach (var episodeType in _fileList)
                {
                    string newName;
                    _currentMappings = _episodeService.GetMappingValues();
                    string oldName = episodeType.FileName;
                    _currentMappings.TryGetValue(oldName, out newName);

                    episodeType.NewName = newName + " " + episodeType.Season.ToUpper() + 
                                          episodeType.Episode.ToUpper();
                    string fileType = _selectedFileType.Type;
                    int found1 = fileType.IndexOf("*");
                    episodeType.FileType = fileType.Remove(found1, 1);
                    
                    _newFileList.Add(episodeType);
                }
                return _newFileList;
            }
        }

        private readonly ObservableCollection<EpisodeType> _fileList;
        private IList<EpisodeType> _soureFileList;
        public ObservableCollection<EpisodeType> FileList
        {
            get
            {
                _fileList.Clear();
                _soureFileList = new List<EpisodeType>(_episodeService.GetSourceEpisode(_sourcePath, _selectedFileType.Type));
                foreach (var episodeType in _soureFileList)
                {
                    _fileList.Add(episodeType);
                }
                return _fileList;
            }

        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _season;
        public string Season
        {
            get { return _season; }
            set { _season = value; }
        }

        private string _episode;
        public string Episode
        {
            get { return _episode; }
            set { _episode = value; }
        }

        private string _releaseGroup;
        public string ReleaseGroup
        {
            get { return _releaseGroup; }
            set { _releaseGroup = value; }
        }

        private string _fileSize;
        public string FileSize
        {
            get { return _fileSize; }
            set { _fileSize = value; }
        }

        private string _fileType;
        public string FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        private EpisodeType _selectedFile;
        public EpisodeType SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                if (_selectedFile != null)
                {
                    _name = SelectedFile.FileName;
                    _season = SelectedFile.Season;
                    _episode = SelectedFile.Episode;
                    _releaseGroup = SelectedFile.ReleaseGroup;
                    _fileSize = SelectedFile.FileSize;
                    _fileType = SelectedFile.FileType;
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Season"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Episode"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ReleaseGroup"));
                    PropertyChanged(this, new PropertyChangedEventArgs("FileSize"));
                    PropertyChanged(this, new PropertyChangedEventArgs("FileType"));
                }
            }
        }
        
        public Dictionary<string, string> CurrentMappings
        {
            get
            {
                _currentMappings = _episodeService.GetMappingValues();
                return _currentMappings;
            }
            
        }
    }
}
﻿namespace GMaster.Views
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Annotations;
    using Camera;

    public class ConnectedCamera : INotifyPropertyChanged
    {
        private bool isAspectAnamorphingVideoOnly;
        private string selectedAspect;
        private LutInfo selectedLut;

        public event PropertyChangedEventHandler PropertyChanged;

        public string[] Aspects => new[] { "1", "1.33", "1.5", "1.75", "2" };

        public Lumix Camera { get; set; }

        public IEnumerable<LutInfo> InstalledLuts => new[] { new LutInfo { Id = null, Title = string.Empty } }.Concat(Model.InstalledLuts);

        public bool IsAspectAnamorphingVideoOnly
        {
            get => isAspectAnamorphingVideoOnly;
            set
            {
                isAspectAnamorphingVideoOnly = value;
                Settings.IsAspectAnamorphingVideoOnly.Value = value;
                OnPropertyChanged();
            }
        }

        public bool IsRemembered => Settings.GeneralSettings.Cameras.Contains(Settings);

        public MainPageModel Model { get; set; }

        public string Name => Camera.Device.FriendlyName;

        public string SelectedAspect
        {
            get => selectedAspect;
            set
            {
                selectedAspect = value;
                Settings.Aspect.Value = value;
                OnPropertyChanged();
            }
        }

        public LutInfo SelectedLut
        {
            get => selectedLut;

            set
            {
                selectedLut = value;
                Settings.LutId.Value = value?.Id;
                OnPropertyChanged();
            }
        }

        public CameraSettings Settings { get; set; }

        public string Udn => Camera.Udn;

        public void Add()
        {
            Settings.GeneralSettings.Cameras.Add(Settings);
            OnPropertyChanged(nameof(IsRemembered));
        }

        public void Remove()
        {
            Settings.GeneralSettings.Cameras.Remove(Settings);
            OnPropertyChanged(nameof(IsRemembered));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
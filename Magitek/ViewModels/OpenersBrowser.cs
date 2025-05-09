﻿using Clio.Utilities.Collections;
using Magitek.Commands;
using Magitek.Gambits;
using Magitek.Utilities;
using Magitek.Utilities.Managers;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Magitek.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class OpenersBrowser
    {
        private static OpenersBrowser _instance;
        public static OpenersBrowser Instance => _instance ?? (_instance = new OpenersBrowser());

        public OpenersBrowser()
        {
            SelectedJob = RotationManager.CurrentRotation.ToString();
            UpdateDisplayedOpeners();
        }

        #region Variables
        public string SelectedJob { get; set; }
        public bool SpinnerVisible { get; set; }
        private readonly HttpClient _webClient = new HttpClient();
        private const string ApiAddress = "https://88x8paere5.execute-api.us-east-1.amazonaws.com/magitek/";
        #endregion

        public AsyncObservableCollection<SharedOpener> Openers { get; set; } = new AsyncObservableCollection<SharedOpener>();

        #region Update Gambits List

        public ICommand UpdateDisplayedGambitsCommand => new AwaitableDelegateCommand(UpdateDisplayedOpenersTask);

        public async void UpdateDisplayedOpeners()
        {
            SpinnerVisible = true;
            await UpdateDisplayedOpenersTask();
            SpinnerVisible = false;
        }

        private async Task UpdateDisplayedOpenersTask()
        {
            try
            {
                var result = await _webClient.GetAsync($@"{ApiAddress}/sharedopeners/job/{SelectedJob}");

                if (!result.IsSuccessStatusCode)
                {
                    return;
                }

                var responseContent = await result.Content.ReadAsStringAsync();
                Openers = new AsyncObservableCollection<SharedOpener>(JsonConvert.DeserializeObject<List<SharedOpener>>(responseContent));
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }

        #endregion

        #region Remove Gambit Group

        public ICommand RemoveOpenerGroup => new AwaitableDelegateCommand<SharedOpener>(async openers =>
        {
            try
            {
                var posterId = "";
                if (posterId == "")
                {
                    return;
                }

                var result = await _webClient.GetAsync($@"{ApiAddress}/sharedopeners/remove/{openers.Id}/{posterId}");

                if (!result.IsSuccessStatusCode)
                {
                    return;
                }

                UpdateDisplayedOpeners();
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        });

        #endregion

        #region Download

        public ICommand DownloadOpenerGroupCommand => new DelegateCommand<string>(openerString =>
        {
            // Check if string is null
            if (string.IsNullOrWhiteSpace(openerString) || string.IsNullOrEmpty(openerString))
                return;

            // Deserialize the string into an opener
            var opener = JsonConvert.DeserializeObject<OpenerGroup>(openerString);

            // Make sure the opener has actions
            if (opener?.Gambits == null)
                return;

            // Generate a new unique ID that doesn't clash with any existing openers
            int newId;
            var random = new Random();
            do
            {
                newId = random.Next(int.MaxValue);
            } while (OpenersViewModel.Instance.OpenerGroups.Any(r => r.Id == newId));

            opener.Id = newId;

            // Add the downloaded opener to the list
            OpenersViewModel.Instance.OpenerGroups.Add(opener);

            // Reset the view source (to update the UI)
            OpenersViewModel.Instance.ResetCollectionViewSource();
        });

        #endregion

        #region Job Selection

        public ICommand JobSelectionChanged => new DelegateCommand<string>(job =>
        {
            if (job == null)
                return;

            SelectedJob = job;
            UpdateDisplayedOpeners();
        });

        #endregion
    }
}

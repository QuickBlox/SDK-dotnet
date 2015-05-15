using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using QMunicate.ViewModels;

namespace QMunicate
{
    internal static class ContinuationManager
    {
        internal static void Continue(IContinuationActivatedEventArgs continuationActivatedEventArgs)
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null) return;
            var page = rootFrame.Content as Page;
            if (page == null) return;

            if (continuationActivatedEventArgs.Kind == ActivationKind.PickFileContinuation)
            {
                var openPickerContinuable = page.DataContext as IFileOpenPickerContinuable;
                var fileOpenArgs = continuationActivatedEventArgs as FileOpenPickerContinuationEventArgs;
                if(openPickerContinuable != null && fileOpenArgs != null)
                    openPickerContinuable.ContinueFileOpenPicker(fileOpenArgs);
            }
        }

    }

    interface IFileOpenPickerContinuable
    {
        /// <summary>
        /// This method is invoked when the file open picker returns picked
        /// files
        /// </summary>
        /// <param name="args">Activated event args object that contains returned files from file open picker</param>
        void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args);
    }



}

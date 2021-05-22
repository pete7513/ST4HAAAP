using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Presentation_Clinician
{
    /// <summary>
    /// Interaction logic for HearingTestWindow.xaml
    /// </summary>
    public partial class HearingTestWindow : Window
    {

        public HearingTestWindow()
        {
            InitializeComponent();

            pdfViewer.ItemSource = @"Audiogram_Patient1.pdf";

        }
    }
}

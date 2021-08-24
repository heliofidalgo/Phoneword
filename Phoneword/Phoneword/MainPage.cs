using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Phoneword
{
    class MainPage : ContentPage
    {
        Entry phoneNumberText;
        Button translateButton;
        Button callButton;
        string translateNumber;

        public MainPage()
        {
            this.Padding = new Thickness(20, 20, 20, 20);

            StackLayout panel = new StackLayout
            {
                Spacing = 15
            };

            panel.Children.Add(new Label
            {
                Text = "Enter a Phoneword here:",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            });

            panel.Children.Add(phoneNumberText = new Entry {
            
                Text = "1-855-XAMARIN",
            });

            panel.Children.Add(translateButton = new Button{
                Text = "Translate",
            });

            panel.Children.Add(callButton =new Button{
                Text = "Call ",
                IsEnabled = false,
            });

            this.Content = panel;

            translateButton.Clicked += OnTranslate;
            callButton.Clicked += OnCall;
            this.Content = panel;
        }

        async void OnCall(object sender, EventArgs e)
        {
           if (await this.DisplayAlert("Dial a Number","Would you like to call" + translateNumber + "?", "Yes", "No"))
            {
                try
                {
                    PhoneDialer.Open(translateNumber);
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing was not supported.", "OK");
                }
                catch (Exception)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
                }
            }
        }

        private void OnTranslate (object sender, EventArgs e)
        {
            string enteredNumber = phoneNumberText.Text;
            translateNumber = Phoneword.PhonewordTranslator.ToNumber(enteredNumber);

            if (!string.IsNullOrEmpty(translateNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = "Call " + translateNumber;  
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Call";
            }
        }
    }
}

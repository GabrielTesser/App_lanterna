using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.Battery.Abstractions;
using Plugin.Battery;



namespace lanterna
{
    public partial class MainPage : ContentPage
    {
        bool lanterna_ligada = false;

        public MainPage()
        {
            InitializeComponent();

            bntOnOff.Source = ImageSource.FromResource("lanterna.Imagens.off.png");

            Carrega_Info_Bateria();
        }

        private void bntOnOff_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (lanterna_ligada == false)
                {
                    Flashlight.TurnOnAsync();
                    bntOnOff.Source = ImageSource.FromResource("lanterna.Imagens.on.png");
                    lanterna_ligada = true;

                }
                else
                {
                    Flashlight.TurnOffAsync();
                    bntOnOff.Source = ImageSource.FromResource("lanterna.Imagens.off.png");
                    lanterna_ligada = false;
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", "Não foi possível acessar a laterna", "OK :(");

            }
            finally
            {
                Vibration.Vibrate(TimeSpan.FromMilliseconds(500));
            }
        }

        private void Mudanca_Status_Bateria(object sender, BatteryChangedEventArgs e)
        {
            try
            {
                // Carga restante.
                lbl_carga_restante.Text = e.RemainingChargePercent.ToString() + "%";

                // Status da Bateria (carregando, etc)
                switch (e.Status)
                {
                    case BatteryStatus.Charging:
                        lbl_status_bateria.Text = "Carregando";
                        break;

                    case BatteryStatus.Discharging:
                        lbl_status_bateria.Text = "Descarregando";
                        break;

                    case BatteryStatus.Full:
                        lbl_status_bateria.Text = "Carga Completa";
                        break;

                    case BatteryStatus.NotCharging:
                        lbl_status_bateria.Text = "Sem Carregar";
                        break;

                    case BatteryStatus.Unknown:
                        lbl_status_bateria.Text = "Desconhecido";
                        break;
                }

                // Fonte de energia do dispositivo
                switch (e.PowerSource)
                {
                    case PowerSource.Ac:
                        lbl_fonte_energia.Text = "Carregador";
                        break;

                    case PowerSource.Battery:
                        lbl_fonte_energia.Text = "Bateria";
                        break;

                    case PowerSource.Other:
                        lbl_fonte_energia.Text = "Outro";
                        break;

                    case PowerSource.Usb:
                        lbl_fonte_energia.Text = "USB";
                        break;

                    case PowerSource.Wireless:
                        lbl_fonte_energia.Text = "Sem fio";
                        break;
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", "Não foi possível obter dados da bateria", "OK");
            }
        }

        private void Carrega_Info_Bateria()
        {
            try
            {
                if (CrossBattery.IsSupported)
                {
                    CrossBattery.Current.BatteryChanged -= Mudanca_Status_Bateria;
                    CrossBattery.Current.BatteryChanged += Mudanca_Status_Bateria;
                }
                else
                    throw new Exception("Não há suporte ao plugin de bateria");

            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", "Não foi possível obter dados da bateria", "OK");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, int> drinks = new Dictionary<string, int>();
        Dictionary<string, int> orders = new Dictionary<string, int>(); 
        public MainWindow()
        {
            InitializeComponent();

            //新增所有飲料品項
            AddNewDrink(drinks);

            //顯示飲料品項菜單
            DisplayDrinkMenu(drinks);
        }

        private void DisplayDrinkMenu(Dictionary<string, int> myDrinks)
        {
            foreach (var drink in myDrinks)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;

                CheckBox cb = new CheckBox();
                cb.Content = $"{drink.Key} : {drink.Value}元";
                cb.Width = 200;
                cb.FontFamily = new FontFamily("Consolas");
                cb.FontSize = 18;
                cb.Foreground = Brushes.Blue;
                cb.Margin = new Thickness(5);

                Slider sd = new Slider();
                sd.Width = 100;
                sd.Value = 0;
                sd.Minimum = 0;
                sd.Maximum = 10;
                Label lb = new Label();
                lb.Width = 50;
                lb.Content = "0";
                lb.FontFamily = new FontFamily("Consolas");
                lb.FontSize = 18;
                lb.Foreground = Brushes.Red;


                sp.Children.Add(cb);
                sp.Children.Add(sd);
                sp.Children.Add(lb);

                stackPanel_DrinkMenu.Children.Add(sp);
            }
        }

        private void AddNewDrink(Dictionary<string, int> mydrinks)
        {
            mydrinks.Add("紅茶大杯", 60);
            mydrinks.Add("紅茶小杯", 40);
            mydrinks.Add("綠茶大杯", 60);
            mydrinks.Add("綠茶小杯", 40);
            mydrinks.Add("咖啡大杯", 80);
            mydrinks.Add("咖啡小杯", 50);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var targetTextBox = sender as TextBox;

            bool success = int.TryParse(targetTextBox.Text, out int amount);

            if (!success) MessageBox.Show("請輸入整數", "輸入錯誤");

            else if (amount <= 0) MessageBox.Show("請輸入正整數", "輸入錯誤");

            else
            {
                StackPanel targetStackPanel = targetTextBox.Parent as StackPanel;
                Label targetLabel = targetStackPanel.Children[0] as Label;
                string drinkname = targetLabel.Content.ToString();
                if(orders.ContainsKey(drinkname)) orders.Remove(drinkname);
                orders.Add(drinkname, amount);
            }
        }

        private void orderbutton_Click(object sender, RoutedEventArgs e)
        {
            double total = 0.0;
            double sellPrice = 0.0;
            string displayString = "訂購清單如下:\n";
            string message = "";
            foreach(KeyValuePair<string, int> item in orders)
            {
                string drinkName = item.Key;
                int amount = orders[drinkName];
                int price = drinks[drinkName];
                total += price * amount;
                displayString += $"{drinkName} X {amount}杯，每杯{price}元，總共{price * amount}元\n";
            }

            if (total >= 500)
            {
                message = "訂購滿500以上者打8折";
                sellPrice = total *  0.8;
            }
            else if(total >=300)
            {
                message = "訂購滿300以上著打85折";
                sellPrice = total *  0.85;
            }
            else if (total >= 200)
            {
                message = "訂購滿200以上者打9折";
                sellPrice = total *  0.9;
            }
            else
            {
                message = "訂購未滿200以上者不打折";
                sellPrice = total;
            }
            displayString += $"本次訂購總共{orders.Count}項，{message}，售價{sellPrice}元";
            TextBlock1.Text = displayString;
        }
    }
}

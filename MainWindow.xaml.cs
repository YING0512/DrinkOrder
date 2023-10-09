using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        string takeout = "";
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
                cb.Margin = new Thickness(5);//線性移動方塊

                Slider sl = new Slider();
                sl.Width = 100;
                sl.Value = 0;
                sl.Minimum = 0;
                sl.Maximum = 10;
                sl.IsSnapToTickEnabled = true;

                Label lb = new Label();
                lb.Width = 50;
                lb.Content = "0";
                lb.FontFamily = new FontFamily("Consolas");
                lb.FontSize = 18;
                lb.Foreground = Brushes.Red;


                sp.Children.Add(cb);
                sp.Children.Add(sl);
                sp.Children.Add(lb);

                //資料細節
                Binding mybinding = new Binding("Value");
                mybinding.Source = sl;
                lb.SetBinding(ContentProperty, mybinding);
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

        private void orderbutton_Click(object sender, RoutedEventArgs e)
        {
            //將訂購飲料加入訂單
            PlaceOrder(orders);
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

        private void PlaceOrder(Dictionary<string, int> myorders)
        {
            myorders.Clear();
            for (int i = 0; i < stackPanel_DrinkMenu.Children.Count; i++)
            {
                StackPanel sp = stackPanel_DrinkMenu.Children[i] as StackPanel;
                CheckBox cb = sp.Children[0] as CheckBox;
                Slider sl = sp.Children[1] as Slider;
                string drinkName = cb.Content.ToString().Substring(0,4);
                int quantity = Convert.ToInt32(sl.Value);

                if (cb.IsChecked == true && quantity != 0)
                {
                    myorders.Add(drinkName,quantity);
                }
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb.IsChecked == true) takeout = rb.Content.ToString();
            MessageBox.Show (takeout);
        }
    }
}

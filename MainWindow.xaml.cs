using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
                StackPanel sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                var cb = new CheckBox
                {
                    Content = $"{drink.Key} : {drink.Value}元",
                    Width = 200,
                    FontFamily = new FontFamily("Consolas"),
                    FontSize = 18,
                    Foreground = Brushes.Blue,
                    Margin = new Thickness(5)//線性移動方塊
                };

                var sl = new Slider
                {
                    Width = 100,
                    Value = 0,
                    Minimum = 0,
                    Maximum = 10,
                    VerticalAlignment = VerticalAlignment.Center,
                    IsSnapToTickEnabled = true
                };

                var lb = new Label
                {
                    Width = 50,
                    Content = "0",
                    FontFamily = new FontFamily("Consolas"),
                    FontSize = 18,
                    Foreground = Brushes.Red
                };


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
            //mydrinks.Add("紅茶大杯", 60);
            //mydrinks.Add("紅茶小杯", 40);
            //mydrinks.Add("綠茶大杯", 60);
            //mydrinks.Add("綠茶小杯", 40);
            //mydrinks.Add("咖啡大杯", 80);
            //mydrinks.Add("咖啡小杯", 50);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV檔案|*.csv|文字檔案|*.txt|全部檔案|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName;
                string[] lines = File.ReadAllLines(filename);
                foreach (var line in lines)
                {
                    string[] tokens = line.Split(',');
                    string drinkName = tokens[0];
                    int price = Convert.ToInt32(tokens[1]);
                    mydrinks.Add(drinkName, price);
                }
            }
        }

        private void orderbutton_Click(object sender, RoutedEventArgs e)
        {
            //將訂購飲料加入訂單
            PlaceOrder(orders);

            //顯示訂單內容
            DisplayOrder(orders);
        }

        private void DisplayOrder(Dictionary<string, int> myorders)
        {
            displayTextBlock.Inlines.Clear();
            Run titleString = new Run
            {
                Text = "您所訂購的飲品為:",
                FontSize = 16,
                Foreground = Brushes.Blue
            };

            Run takeoutString = new Run
            {
                Text = $"{takeout}",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
            };

            displayTextBlock.Inlines.Add(titleString);
            displayTextBlock.Inlines.Add(takeoutString);
            displayTextBlock.Inlines.Add(new Run() { Text = "，訂購明細如下: \n", FontSize = 16 });


            double total = 0.0;
            double sellprice = 0.0;
            //string displayString = "訂購清單如下:\n";
            string discountString = "";
            int i = 1;

            foreach (var item in myorders)
            {
                string drinkName = item.Key;
                int quantity = myorders[drinkName];
                int price = drinks[drinkName];
                total += price * quantity;
                displayTextBlock.Inlines.Add(new Run() { Text = $"飲料品項{i} :{drinkName} X {quantity}杯，每杯{price}元，總共{price * quantity}元\n" });
                i++;
                //displayString += $"{drinkName} X {quantity}杯，每杯{price}元，總共{price * quantity}元\n";
            }

            if (total >= 500)
            {
                discountString = "訂購滿500以上者打8折";
                sellprice = total * 0.8;
            }
            else if (total >= 300)
            {
                discountString = "訂購滿300以上著打85折";
                sellprice = total * 0.85;
            }
            else if (total >= 200)
            {
                discountString = "訂購滿200以上者打9折";
                sellprice = total * 0.9;
            }
            else
            {
                discountString = "訂購未滿200以上者不打折";
                sellprice = total;
            }
            Italic summaString = new Italic( new Run
            {
                Text = $"本次訂購總共{myorders.Count}項，{discountString}，售價{sellprice}元",
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Foreground =Brushes.Red
            });
            displayTextBlock.Inlines.Add(summaString);
            //displayString += $"";
            //displayTextBlock.Text = displayString;

        }

        private void PlaceOrder(Dictionary<string, int> myorders)
        {
            myorders.Clear();
            for (int i = 0; i < stackPanel_DrinkMenu.Children.Count; i++)
            {
                var sp = stackPanel_DrinkMenu.Children[i] as StackPanel;
                var cb = sp.Children[0] as CheckBox;
                var sl = sp.Children[1] as Slider;
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

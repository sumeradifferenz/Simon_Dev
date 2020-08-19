using System;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Diagnostics;

namespace Simon.Controls
{
    public class RepeaterView : StackLayout
    {
        #region Bindable Properties

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(propertyName: nameof(ItemTemplate), returnType: typeof(DataTemplate), declaringType: typeof(RepeaterView), defaultValue: default(DataTemplate), propertyChanged: OnItemTemplateChanged);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(propertyName: nameof(ItemsSource), returnType: typeof(IEnumerable<object>), declaringType: typeof(RepeaterView), propertyChanged: OnItemsSourceChanged);

        public static readonly BindableProperty ColumnCountProperty = BindableProperty.Create(propertyName: nameof(ColumnCount), returnType: typeof(double), declaringType: typeof(RepeaterView), defaultValue: double.MinValue, propertyChanged: OnColumnCountChanged);

        public static readonly BindableProperty GridWidthUnitTypeProperty = BindableProperty.Create(propertyName: nameof(GridWidthUnitType), returnType: typeof(GridUnitType), declaringType: typeof(RepeaterView), defaultValue: GridUnitType.Star, propertyChanged: OnGridWidthUnitTypeChanged);

        public static readonly BindableProperty GridHeightUnitTypeProperty = BindableProperty.Create(propertyName: nameof(GridHeightUnitType), returnType: typeof(GridUnitType), declaringType: typeof(RepeaterView), defaultValue: GridUnitType.Star, propertyChanged: OnGridHeightUnitTypeChanged);

        #endregion

        #region Properties 

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public double ColumnCount
        {
            get { return (double)GetValue(ColumnCountProperty); }
            set { SetValue(ColumnCountProperty, value); }
        }

        public GridUnitType GridWidthUnitType
        {
            get { return (GridUnitType)GetValue(GridWidthUnitTypeProperty); }
            set { SetValue(GridWidthUnitTypeProperty, value); }
        }

        public GridUnitType GridHeightUnitType
        {
            get { return (GridUnitType)GetValue(GridHeightUnitTypeProperty); }
            set { SetValue(GridHeightUnitTypeProperty, value); }
        }

        #endregion

        #region Property Changed Callbacks

        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RepeaterView)bindable;
            if (newValue == null) { return; }
            layout.PopulateItems();
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RepeaterView)bindable;
            if (newValue == null)
            {
                layout.Children.Clear();
                return;
            }

            layout.PopulateItems();
        }

        private static void OnColumnCountChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RepeaterView)bindable;
            if (newValue == null) { return; }

            layout.PopulateItems();
        }

        private static void OnGridWidthUnitTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RepeaterView)bindable;
            if (newValue == null) { return; }

            layout.PopulateItems();
        }

        private static void OnGridHeightUnitTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var layout = (RepeaterView)bindable;
            if (newValue == null) { return; }
            layout.PopulateItems();
        }

        #endregion

        #region Private Methods

        private void PopulateItems()
        {
            try
            {
                var items = ItemsSource;
                if (items == null || ItemTemplate == null) { return; }

                var children = Children;
                children?.Clear();

                if (ColumnCount > 0)
                {
                    var ViewCollection = items.ToList();

                    ColumnDefinitionCollection columnDefinitions = new ColumnDefinitionCollection();
                    RowDefinitionCollection rowDefinitions = new RowDefinitionCollection();

                    for (int i = 0; i < ColumnCount; i++)
                    {
                        columnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridWidthUnitType) });
                    }

                    for (int i = 0; i < ViewCollection.Count; i++)
                    {
                        rowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridHeightUnitType) });
                    }

                    var grd = new Grid()
                    {
                        ColumnSpacing = this.Spacing,
                        RowSpacing = this.Spacing,
                        Padding = 0
                    };

                    if (GridHeightUnitType == GridUnitType.Auto)
                    {
                        grd.RowDefinitions = rowDefinitions;
                    }

                    if (GridWidthUnitType == GridUnitType.Auto)
                    {
                        grd.ColumnDefinitions = columnDefinitions;
                    }

                    var row = 0;
                    for (int i = 0; i < ViewCollection.Count; i++)
                    {
                        for (int j = 0; j < ColumnCount; j++)
                        {
                            if (i < ViewCollection.Count && ViewCollection[i] != null)
                            {
                                var item = ViewCollection[i];
                                grd.Children.Add(InflateView(item), j, row);

                                if (j != ColumnCount - 1)
                                    i++;
                            }
                        }
                        row++;
                    }
                    children.Add(grd);
                }
                else
                {
                    foreach (var item in items)
                    {
                        children.Add(InflateView(item));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private View InflateView(object viewModel)
        {
            try
            {
                var view = (View)CreateContent(ItemTemplate, viewModel, this);
                view.BindingContext = viewModel;
                return view;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        #endregion

        #region Static Methods

        public static object CreateContent(DataTemplate template, object item, BindableObject container)
        {
            try
            {
                var selector = template as DataTemplateSelector;
                if (selector != null)
                {
                    template = selector.SelectTemplate(item, container);
                }
                return template.CreateContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        #endregion
    }
}

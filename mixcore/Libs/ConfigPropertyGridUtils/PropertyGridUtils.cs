﻿using System;
using System.ComponentModel;

namespace PropertyGridUtils
{
    /// <summary>
    /// Атрибут для поддержки динамически показываемых свойств
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DynamicPropertyFilterAttribute : Attribute
    {
        string _propertyName;

        /// <summary>
        /// Название свойства, от которого будет зависить видимость  
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }

        string _showOn;

        /// <summary>
        /// Значения свойства от которого зависит видимость 
        /// (через запятую, если несколько), при котором свойство, к
        /// которому применен атрибут, будет видимо. 
        /// </summary>
        public string ShowOn
        {
            get { return _showOn; }
        }

        /// <summary>
        /// Конструктор  
        /// </summary>
        /// <param name="propName">Название свойства, от которого будет зависить видимость</param>
        /// <param name="value">Значения свойства, через запятую, если несколько, при котором свойство, к
        /// которому применен атрибут, будет видимо.</param>
        public DynamicPropertyFilterAttribute(string propName, string value)
        {
            _propertyName = propName;
            _showOn = value;
        }
    }

    /// <summary>
    /// Базовый класс для объектов, поддерживающих динамическое 
    /// отображение свойств в PropertyGrid
    /// </summary>
    public class FilterablePropertyBase : ICustomTypeDescriptor
    {

        protected PropertyDescriptorCollection
        GetFilteredProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection pdc
            = TypeDescriptor.GetProperties(this, attributes, true);
            PropertyDescriptorCollection finalProps =
               new PropertyDescriptorCollection(new PropertyDescriptor[0]);

            foreach (PropertyDescriptor pd in pdc)
            {
                bool include = false;
                bool Dynamic = false;
                foreach (Attribute a in pd.Attributes)
                {
                    if (a is DynamicPropertyFilterAttribute)
                    {
                        Dynamic = true;

                        DynamicPropertyFilterAttribute
                        dpf = (DynamicPropertyFilterAttribute)a;
                        PropertyDescriptor temp = pdc[dpf.PropertyName];

                        if (dpf.ShowOn.IndexOf(temp.GetValue(this).ToString()) > -1)
                        {
                            include = true;
                        }
                    }
                }

                if (!Dynamic || include)
                    finalProps.Add(pd);
            }

            return finalProps;
        }

        #region ICustomTypeDescriptor Members

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public PropertyDescriptorCollection GetProperties(
           Attribute[] attributes)
        {
            return GetFilteredProperties(attributes);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return GetFilteredProperties(new Attribute[0]);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        #endregion

    }

}

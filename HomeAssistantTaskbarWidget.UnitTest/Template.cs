using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core.Exceptions;
using HomeAssistantTaskbarWidget.Model;
using HomeAssistantTaskbarWidget.Utils;

namespace HomeAssistantTaskbarWidget.UnitTest
{
    [TestClass]
    public class Template
    {
        #region Consts

        private readonly Entity Entity = new Entity
        {
            entity_id = "sensor.temperature_1_out",
            state =  "4.7",
            attributes =  new Attributes {
                state_class =  "measurement",
                unit_of_measurement =  "°C", 
                device_class =  "temperature",
                friendly_name =  "Temperature #1 out"
            },
            last_changed =  DateTime.Parse("2022-01-26T08:54:22.074431+00:00"),
            last_updated = DateTime.Parse("2022-01-26T08:54:22.074431+00:00"),
            context = new Context {
                id =  "3946d3be868c553b1a45449a0b677685",
                parent_id =  null,
                user_id =  null
            }
        };

        private readonly List<Entity> ListEntity = new List<Entity>
        {
            new Entity
            {
                entity_id = "sensor.temperature_1_out",
                state =  "4.7",
                attributes =  new Attributes {
                    state_class =  "measurement",
                    unit_of_measurement =  "°C",
                    device_class =  "temperature",
                    friendly_name =  "Temperature #1 out"
                },
                last_changed =  DateTime.Parse("2022-01-26T08:54:22.074431+00:00"),
                last_updated = DateTime.Parse("2022-01-26T08:54:22.074431+00:00"),
                context = new Context {
                    id =  "3946d3be868c553b1a45449a0b677685",
                    parent_id =  null,
                    user_id =  null
                }
            },
            new Entity
            {
                entity_id = "sensor.temperature_2_in",
                state =  "5.2",
                attributes =  new Attributes {
                    state_class =  "measurement",
                    unit_of_measurement =  "°C",
                    device_class =  "temperature",
                    friendly_name =  "Temperature #2 in"
                },
                last_changed =  DateTime.Parse("2022-01-26T08:54:22.074431+00:00"),
                last_updated = DateTime.Parse("2022-01-26T08:54:22.074431+00:00"),
                context = new Context {
                    id =  "3946d3be868c553b1a45449a0b677685",
                    parent_id =  null,
                    user_id =  null
                }
            }
        };
        #endregion

        [TestMethod]
        public void CorrectTemplate()
        {
            var template = "{entity.attributes.friendly_name} => {entity.state}";

            var result = Helper.ReplaceTemplate(template, Entity);

            Assert.AreEqual("Temperature #1 out => 4.7", result);
        }

        [TestMethod]
        public void IncorrectPropInTemplate()
        {
            var template = "{entity.attribute.friendly_name} => {entity.state}";

            Action action = () => Helper.ReplaceTemplate(template, Entity);

            Assert.ThrowsException<ParseException>(action);
        }

        [TestMethod]
        public void CorrectWithMethod()
        {
            var template = "{entity.attributes.friendly_name} Updated: {entity.last_updated.ToShortDateString()}";

            var result = Helper.ReplaceTemplate(template, Entity);

            Assert.AreEqual("Temperature #1 out Updated: 26.01.2022", result);
        }

        [TestMethod]
        public void IncorrectMethodInTemplate()
        {
            var template = "{entity.attributes.friendly_name} Updated: {entity.last_updated.ToShortDate()}";

            Action action = () => Helper.ReplaceTemplate(template, Entity);

            Assert.ThrowsException<ParseException>(action);
        }

        [TestMethod]
        public void CorrectWith3Props()
        {
            var template = "{entity.attributes.friendly_name}: {entity.state} {entity.attributes.unit_of_measurement}";

            var result = Helper.ReplaceTemplate(template, Entity);

            Assert.AreEqual("Temperature #1 out: 4.7 °C", result);
        }


        [TestMethod]
        public void ListCorrectTemplate()
        {
            var template = "{items[1].attributes.friendly_name} => {items[0].state}";

            var result = Helper.ReplaceTemplate(template, ListEntity);

            Assert.AreEqual("Temperature #2 in => 4.7", result);
        }

        [TestMethod]
        public void ListIncorrectPropInTemplate()
        {
            var template = "{items[0].attribute.friendly_name} => {items[3].state}";

            Action action = () => Helper.ReplaceTemplate(template, ListEntity);

            Assert.ThrowsException<ParseException>(action);
        }

        [TestMethod]
        public void ListCorrectWithMethod()
        {
            var template = "{items[0].attributes.friendly_name} Updated: {items[0].last_updated.ToShortDateString()}";

            var result = Helper.ReplaceTemplate(template, ListEntity);

            Assert.AreEqual("Temperature #1 out Updated: 26.01.2022", result);
        }

        [TestMethod]
        public void ListIncorrectMethodInTemplate()
        {
            var template = "{items[0].attributes.friendly_name} Updated: {items[0].last_updated.ToShortDate()}";

            Action action = () => Helper.ReplaceTemplate(template, ListEntity);

            Assert.ThrowsException<ParseException>(action);
        }

        [TestMethod]
        public void ListCorrectWith3Props()
        {
            var template = "{items[0].attributes.friendly_name}: {items[1].state} {items[0].attributes.unit_of_measurement}";

            var result = Helper.ReplaceTemplate(template, ListEntity);

            Assert.AreEqual("Temperature #1 out: 5.2 °C", result);
        }
    }
}

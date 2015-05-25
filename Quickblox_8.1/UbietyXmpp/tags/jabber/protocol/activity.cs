// activity.cs
//
//Copyright © 2006 - 2012 Dieter Lunn
//Modified 2012 Paul Freund ( freund.paul@lvl3.org )
//
//This library is free software; you can redistribute it and/or modify it under
//the terms of the GNU Lesser General Public License as published by the Free
//Software Foundation; either version 3 of the License, or (at your option)
//any later version.
//
//This library is distributed in the hope that it will be useful, but WITHOUT
//ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
//FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
//
//You should have received a copy of the GNU Lesser General Public License along
//with this library; if not, write to the Free Software Foundation, Inc., 59
//Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System.Collections.Generic;
using System.Xml.Linq;
using XMPP.registries;

namespace XMPP.tags.jabber.protocol.activity
{
    public class Namespace
    {
        public static string Name = "http://jabber.org/protocol/activity";
        public static XName activity = XName.Get("activity", Name);
        public static XName doing_chores = XName.Get("doing_chores", Name);
        public static XName drinking = XName.Get("drinking", Name);
        public static XName eating = XName.Get("eating", Name);
        public static XName exercising = XName.Get("exercising", Name);
        public static XName grooming = XName.Get("grooming", Name);
        public static XName having_appointment = XName.Get("having_appointment", Name);
        public static XName inactive = XName.Get("inactive", Name);
        public static XName relaxing = XName.Get("relaxing", Name);
        public static XName talking = XName.Get("talking", Name);
        public static XName traveling = XName.Get("traveling", Name);
        public static XName working = XName.Get("working", Name);
        public static XName text = XName.Get("text", Name);
        public static XName at_the_spa = XName.Get("at_the_spa", Name);
        public static XName brushing_teeth = XName.Get("brushing_teeth", Name);
        public static XName buying_groceries = XName.Get("buying_groceries", Name);
        public static XName cleaning = XName.Get("cleaning", Name);
        public static XName coding = XName.Get("coding", Name);
        public static XName commuting = XName.Get("commuting", Name);
        public static XName cooking = XName.Get("cooking", Name);
        public static XName cycling = XName.Get("cycling", Name);
        public static XName day_off = XName.Get("day_off", Name);
        public static XName doing_maintenance = XName.Get("doing_maintenance", Name);
        public static XName doing_the_dishes = XName.Get("doing_the_dishes", Name);
        public static XName doing_the_laundry = XName.Get("doing_the_laundry", Name);
        public static XName driving = XName.Get("driving", Name);
        public static XName gaming = XName.Get("gaming", Name);
        public static XName gardening = XName.Get("gardening", Name);
        public static XName getting_a_haircut = XName.Get("getting_a_haircut", Name);
        public static XName going_out = XName.Get("going_out", Name);
        public static XName hanging_out = XName.Get("hanging_out", Name);
        public static XName having_a_beer = XName.Get("having_a_beer", Name);
        public static XName having_a_snack = XName.Get("having_a_snack", Name);
        public static XName having_breakfast = XName.Get("having_breakfast", Name);
        public static XName having_coffee = XName.Get("having_coffee", Name);
        public static XName having_dinner = XName.Get("having_dinner", Name);
        public static XName having_lunch = XName.Get("having_lunch", Name);
        public static XName having_tea = XName.Get("having_tea", Name);
        public static XName hiking = XName.Get("hiking", Name);
        public static XName in_a_car = XName.Get("in_a_car", Name);
        public static XName in_a_meeting = XName.Get("in_a_meeting", Name);
        public static XName in_real_life = XName.Get("in_real_life", Name);
        public static XName jogging = XName.Get("jogging", Name);
        public static XName on_a_bus = XName.Get("on_a_bus", Name);
        public static XName on_a_plane = XName.Get("on_a_plane", Name);
        public static XName on_a_train = XName.Get("on_a_train", Name);
        public static XName on_a_trip = XName.Get("on_a_trip", Name);
        public static XName on_the_phone = XName.Get("on_the_phone", Name);
        public static XName on_vacation = XName.Get("on_vacation", Name);
        public static XName other = XName.Get("other", Name);
        public static XName partying = XName.Get("partying", Name);
        public static XName playing_sports = XName.Get("playing_sports", Name);
        public static XName reading = XName.Get("reading", Name);
        public static XName rehearsing = XName.Get("rehearsing", Name);
        public static XName running = XName.Get("running", Name);
        public static XName running_an_errand = XName.Get("running_an_errand", Name);
        public static XName scheduled_holiday = XName.Get("scheduled_holiday", Name);
        public static XName shaving = XName.Get("shaving", Name);
        public static XName shopping = XName.Get("shopping", Name);
        public static XName skiing = XName.Get("skiing", Name);
        public static XName sleeping = XName.Get("sleeping", Name);
        public static XName socializing = XName.Get("socializing", Name);
        public static XName studying = XName.Get("studying", Name);
        public static XName sunbathing = XName.Get("sunbathing", Name);
        public static XName swimming = XName.Get("swimming", Name);
        public static XName taking_a_bath = XName.Get("taking_a_bath", Name);
        public static XName taking_a_shower = XName.Get("taking_a_shower", Name);
        public static XName walking = XName.Get("walking", Name);
        public static XName walking_the_dog = XName.Get("walking_the_dog", Name);
        public static XName watching_tv = XName.Get("watching_tv", Name);
        public static XName watching_a_movie = XName.Get("watching_a_movie", Name);
        public static XName working_out = XName.Get("working_out", Name);
        public static XName writing = XName.Get("writing", Name);
    }


    [XMPPTag(typeof(Namespace), typeof(activity))] 
    public class activity : Tag
    { 
        public activity() : base(Namespace.activity) {} 
        public activity(XElement other) : base(other) {} 

        public IEnumerable<doing_chores> doing_choresElements { get { return Elements<doing_chores>(Namespace.doing_chores); } }
        public IEnumerable<drinking> drinkingElements { get { return Elements<drinking>(Namespace.drinking); } }
        public IEnumerable<eating> eatingElements { get { return Elements<eating>(Namespace.eating); } }
        public IEnumerable<exercising> exercisingElements { get { return Elements<exercising>(Namespace.exercising); } }
        public IEnumerable<grooming> groomingElements { get { return Elements<grooming>(Namespace.grooming); } }
        public IEnumerable<having_appointment> having_appointmentElements { get { return Elements<having_appointment>(Namespace.having_appointment); } }
        public IEnumerable<inactive> inactiveElements { get { return Elements<inactive>(Namespace.inactive); } }
        public IEnumerable<relaxing> relaxingElements { get { return Elements<relaxing>(Namespace.relaxing); } }
        public IEnumerable<talking > talkingElements { get { return Elements<talking>(Namespace.talking); } }
        public IEnumerable<traveling> travelingElements { get { return Elements<traveling>(Namespace.traveling); } }
        public IEnumerable<working > workingElements { get { return Elements<working >(Namespace.working ); } }
        public IEnumerable<text> textElements { get { return Elements<text>(Namespace.text); } }
    }

    [XMPPTag(typeof(Namespace), typeof(text))]
    public class text : Tag 
    { 
        public text() : base(Namespace.text) {} 
        public text(XElement other) : base(other) {} 
    }

    [XMPPTag(typeof(Namespace), typeof(doing_chores))]
    public class doing_chores : Tag
	{
		public doing_chores() : base(Namespace.doing_chores) {} 
		public doing_chores(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(drinking))]
    public class drinking : Tag
	{
		public drinking() : base(Namespace.drinking) {} 
		public drinking(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(eating))]
    public class eating : Tag
	{
		public eating() : base(Namespace.eating) {} 
		public eating(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(exercising))]
    public class exercising : Tag
	{
		public exercising() : base(Namespace.exercising) {} 
		public exercising(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(grooming))]
    public class grooming : Tag
	{
		public grooming() : base(Namespace.grooming) {} 
		public grooming(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(having_appointment))]
    public class having_appointment : Tag
	{
		public having_appointment() : base(Namespace.having_appointment) {} 
		public having_appointment(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(inactive))]
    public class inactive : Tag
	{
		public inactive() : base(Namespace.inactive) {} 
		public inactive(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(relaxing))]
    public class relaxing : Tag
	{
		public relaxing() : base(Namespace.relaxing) {} 
		public relaxing(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(talking))]
    public class talking : Tag
	{
		public talking() : base(Namespace.talking) {} 
		public talking(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(traveling))]
    public class traveling : Tag
	{
		public traveling() : base(Namespace.traveling) {} 
		public traveling(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(working))]
    public class working : Tag
	{
		public working() : base(Namespace.working) {} 
		public working(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(at_the_spa))]
    public class at_the_spa : Tag
	{
		public at_the_spa() : base(Namespace.at_the_spa) {} 
		public at_the_spa(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(brushing_teeth))]
    public class brushing_teeth : Tag
	{
		public brushing_teeth() : base(Namespace.brushing_teeth) {} 
		public brushing_teeth(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(buying_groceries))]
    public class buying_groceries : Tag
	{
		public buying_groceries() : base(Namespace.buying_groceries) {} 
		public buying_groceries(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(cleaning))]
    public class cleaning : Tag
	{
		public cleaning() : base(Namespace.cleaning) {} 
		public cleaning(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(coding))]
    public class coding : Tag
	{
		public coding() : base(Namespace.coding) {} 
		public coding(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(commuting))]
    public class commuting : Tag
	{
		public commuting() : base(Namespace.commuting) {} 
		public commuting(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(cooking))]
    public class cooking : Tag
	{
		public cooking() : base(Namespace.cooking) {} 
		public cooking(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(cycling))]
    public class cycling : Tag
	{
		public cycling() : base(Namespace.cycling) {} 
		public cycling(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(day_off))]
    public class day_off : Tag
	{
		public day_off() : base(Namespace.day_off) {} 
		public day_off(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(doing_maintenance))]
    public class doing_maintenance : Tag
	{
		public doing_maintenance() : base(Namespace.doing_maintenance) {} 
		public doing_maintenance(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(doing_the_dishes))]
    public class doing_the_dishes : Tag
	{
		public doing_the_dishes() : base(Namespace.doing_the_dishes) {} 
		public doing_the_dishes(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(doing_the_laundry))]
    public class doing_the_laundry : Tag
	{
		public doing_the_laundry() : base(Namespace.doing_the_laundry) {} 
		public doing_the_laundry(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(driving))]
    public class driving : Tag
	{
		public driving() : base(Namespace.driving) {} 
		public driving(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(gaming))]
    public class gaming : Tag
	{
		public gaming() : base(Namespace.gaming) {} 
		public gaming(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(gardening))]
    public class gardening : Tag
	{
		public gardening() : base(Namespace.gardening) {} 
		public gardening(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(getting_a_haircut))]
    public class getting_a_haircut : Tag
	{
		public getting_a_haircut() : base(Namespace.getting_a_haircut) {} 
		public getting_a_haircut(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(going_out))]
    public class going_out : Tag
	{
		public going_out() : base(Namespace.going_out) {} 
		public going_out(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(hanging_out))]
    public class hanging_out : Tag
	{
		public hanging_out() : base(Namespace.hanging_out) {} 
		public hanging_out(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(having_a_beer))]
    public class having_a_beer : Tag
	{
		public having_a_beer() : base(Namespace.having_a_beer) {} 
		public having_a_beer(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(having_a_snack))]
    public class having_a_snack : Tag
	{
		public having_a_snack() : base(Namespace.having_a_snack) {} 
		public having_a_snack(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(having_breakfast))]
    public class having_breakfast : Tag
	{
		public having_breakfast() : base(Namespace.having_breakfast) {} 
		public having_breakfast(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(having_coffee))]
    public class having_coffee : Tag
	{
		public having_coffee() : base(Namespace.having_coffee) {} 
		public having_coffee(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(having_dinner))]
    public class having_dinner : Tag
	{
		public having_dinner() : base(Namespace.having_dinner) {} 
		public having_dinner(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(having_lunch))]
    public class having_lunch : Tag
	{
		public having_lunch() : base(Namespace.having_lunch) {} 
		public having_lunch(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(having_tea))]
    public class having_tea : Tag
	{
		public having_tea() : base(Namespace.having_tea) {} 
		public having_tea(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(hiking))]
    public class hiking : Tag
	{
		public hiking() : base(Namespace.hiking) {} 
		public hiking(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(in_a_car))]
    public class in_a_car : Tag
	{
		public in_a_car() : base(Namespace.in_a_car) {} 
		public in_a_car(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(in_a_meeting))]
    public class in_a_meeting : Tag
	{
		public in_a_meeting() : base(Namespace.in_a_meeting) {} 
		public in_a_meeting(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(in_real_life))]
    public class in_real_life : Tag
	{
		public in_real_life() : base(Namespace.in_real_life) {} 
		public in_real_life(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(jogging))]
    public class jogging : Tag
	{
		public jogging() : base(Namespace.jogging) {} 
		public jogging(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(on_a_bus))]
    public class on_a_bus : Tag
	{
		public on_a_bus() : base(Namespace.on_a_bus) {} 
		public on_a_bus(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(on_a_plane))]
    public class on_a_plane : Tag
	{
		public on_a_plane() : base(Namespace.on_a_plane) {} 
		public on_a_plane(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(on_a_train))]
    public class on_a_train : Tag
	{
		public on_a_train() : base(Namespace.on_a_train) {} 
		public on_a_train(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(on_a_trip))]
    public class on_a_trip : Tag
	{
		public on_a_trip() : base(Namespace.on_a_trip) {} 
		public on_a_trip(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(on_the_phone))]
    public class on_the_phone : Tag
	{
		public on_the_phone() : base(Namespace.on_the_phone) {} 
		public on_the_phone(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(on_vacation))]
    public class on_vacation : Tag
	{
		public on_vacation() : base(Namespace.on_vacation) {} 
		public on_vacation(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(other))]
    public class other : Tag
	{
		public other() : base(Namespace.other) {} 
		public other(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(partying))]
    public class partying : Tag
	{
		public partying() : base(Namespace.partying) {} 
		public partying(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(playing_sports))]
    public class playing_sports : Tag
	{
		public playing_sports() : base(Namespace.playing_sports) {} 
		public playing_sports(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(reading))]
    public class reading : Tag
	{
		public reading() : base(Namespace.reading) {} 
		public reading(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(rehearsing))]
    public class rehearsing : Tag
	{
		public rehearsing() : base(Namespace.rehearsing) {} 
		public rehearsing(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(running))]
    public class running : Tag
	{
		public running() : base(Namespace.running) {} 
		public running(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(running_an_errand))]
    public class running_an_errand : Tag
	{
		public running_an_errand() : base(Namespace.running_an_errand) {} 
		public running_an_errand(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(scheduled_holiday))]
    public class scheduled_holiday : Tag
	{
		public scheduled_holiday() : base(Namespace.scheduled_holiday) {} 
		public scheduled_holiday(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(shaving))]
    public class shaving : Tag
	{
		public shaving() : base(Namespace.shaving) {} 
		public shaving(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(shopping))]
    public class shopping : Tag
	{
		public shopping() : base(Namespace.shopping) {} 
		public shopping(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(skiing))]
    public class skiing : Tag
	{
		public skiing() : base(Namespace.skiing) {} 
		public skiing(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(sleeping))]
    public class sleeping : Tag
	{
		public sleeping() : base(Namespace.sleeping) {} 
		public sleeping(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(socializing))]
    public class socializing : Tag
	{
		public socializing() : base(Namespace.socializing) {} 
		public socializing(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(studying))]
    public class studying : Tag
	{
		public studying() : base(Namespace.studying) {} 
		public studying(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(sunbathing))]
    public class sunbathing : Tag
	{
		public sunbathing() : base(Namespace.sunbathing) {} 
		public sunbathing(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(swimming))]
    public class swimming : Tag
	{
		public swimming() : base(Namespace.swimming) {} 
		public swimming(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(taking_a_bath))]
    public class taking_a_bath : Tag
	{
		public taking_a_bath() : base(Namespace.taking_a_bath) {} 
		public taking_a_bath(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(taking_a_shower))]
    public class taking_a_shower : Tag
	{
		public taking_a_shower() : base(Namespace.taking_a_shower) {} 
		public taking_a_shower(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(walking))]
    public class walking : Tag
	{
		public walking() : base(Namespace.walking) {} 
		public walking(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(walking_the_dog))]
    public class walking_the_dog : Tag
	{
		public walking_the_dog() : base(Namespace.walking_the_dog) {} 
		public walking_the_dog(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(watching_tv))]
    public class watching_tv : Tag
	{
		public watching_tv() : base(Namespace.watching_tv) {} 
		public watching_tv(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(watching_a_movie))]
    public class watching_a_movie : Tag
	{
		public watching_a_movie() : base(Namespace.watching_a_movie) {} 
		public watching_a_movie(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(working_out))]
    public class working_out : Tag
	{
		public working_out() : base(Namespace.working_out) {} 
		public working_out(XElement other) : base(other) {}
	}

    [XMPPTag(typeof(Namespace), typeof(writing))]
    public class writing : Tag
	{
		public writing() : base(Namespace.writing) {} 
		public writing(XElement other) : base(other) {}
	}
}

/*
<?xml version='1.0' encoding='UTF-8'?>

<xs:schema
    xmlns:xs='http://www.w3.org/2001/XMLSchema'
    targetNamespace='http://jabber.org/protocol/activity'
    xmlns='http://jabber.org/protocol/activity'
    elementFormDefault='qualified'>

  <xs:annotation>
    <xs:documentation>
      The protocol documented by this schema is defined in
      XEP-0108: http://www.xmpp.org/extensions/xep-0108.html
    </xs:documentation>
  </xs:annotation>

  <xs:element name='activity'>
    <xs:complexType>
      <xs:sequence>
        <xs:choice>
          <xs:element name='doing_chores' type='general'/>
          <xs:element name='drinking' type='general'/>
          <xs:element name='eating' type='general'/>
          <xs:element name='exercising' type='general'/>
          <xs:element name='grooming' type='general'/>
          <xs:element name='having_appointment' type='general'/>
          <xs:element name='inactive' type='general'/>
          <xs:element name='relaxing' type='general'/>
          <xs:element name='talking' type='general'/>
          <xs:element name='traveling' type='general'/>
          <xs:element name='working' type='general'/>
        </xs:choice>
        <xs:element name='text' minOccurs='0' type='xs:string'/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:complexType name='general'>
    <xs:choice minOccurs='0'>
      <xs:choice minOccurs='0'>
        <xs:element name='at_the_spa' type='specific'/>
        <xs:element name='brushing_teeth' type='specific'/>
        <xs:element name='buying_groceries' type='specific'/>
        <xs:element name='cleaning' type='specific'/>
        <xs:element name='coding' type='specific'/>
        <xs:element name='commuting' type='specific'/>
        <xs:element name='cooking' type='specific'/>
        <xs:element name='cycling' type='specific'/>
        <xs:element name='day_off' type='specific'/>
        <xs:element name='doing_maintenance' type='specific'/>
        <xs:element name='doing_the_dishes' type='specific'/>
        <xs:element name='doing_the_laundry' type='specific'/>
        <xs:element name='driving' type='specific'/>
        <xs:element name='gaming' type='specific'/>
        <xs:element name='gardening' type='specific'/>
        <xs:element name='getting_a_haircut' type='specific'/>
        <xs:element name='going_out' type='specific'/>
        <xs:element name='hanging_out' type='specific'/>
        <xs:element name='having_a_beer' type='specific'/>
        <xs:element name='having_a_snack' type='specific'/>
        <xs:element name='having_breakfast' type='specific'/>
        <xs:element name='having_coffee' type='specific'/>
        <xs:element name='having_dinner' type='specific'/>
        <xs:element name='having_lunch' type='specific'/>
        <xs:element name='having_tea' type='specific'/>
        <xs:element name='hiking' type='specific'/>
        <xs:element name='in_a_car' type='specific'/>
        <xs:element name='in_a_meeting' type='specific'/>
        <xs:element name='in_real_life' type='specific'/>
        <xs:element name='jogging' type='specific'/>
        <xs:element name='on_a_bus' type='specific'/>
        <xs:element name='on_a_plane' type='specific'/>
        <xs:element name='on_a_train' type='specific'/>
        <xs:element name='on_a_trip' type='specific'/>
        <xs:element name='on_the_phone' type='specific'/>
        <xs:element name='on_vacation' type='specific'/>
        <xs:element name='other' type='specific'/>
        <xs:element name='partying' type='specific'/>
        <xs:element name='playing_sports' type='specific'/>
        <xs:element name='reading' type='specific'/>
        <xs:element name='rehearsing' type='specific'/>
        <xs:element name='running' type='specific'/>
        <xs:element name='running_an_errand' type='specific'/>
        <xs:element name='scheduled_holiday' type='specific'/>
        <xs:element name='shaving' type='specific'/>
        <xs:element name='shopping' type='specific'/>
        <xs:element name='skiing' type='specific'/>
        <xs:element name='sleeping' type='specific'/>
        <xs:element name='socializing' type='specific'/>
        <xs:element name='studying' type='specific'/>
        <xs:element name='sunbathing' type='specific'/>
        <xs:element name='swimming' type='specific'/>
        <xs:element name='taking_a_bath' type='specific'/>
        <xs:element name='taking_a_shower' type='specific'/>
        <xs:element name='walking' type='specific'/>
        <xs:element name='walking_the_dog' type='specific'/>
        <xs:element name='watching_tv' type='specific'/>
        <xs:element name='watching_a_movie' type='specific'/>
        <xs:element name='working_out' type='specific'/>
        <xs:element name='writing' type='specific'/>
      </xs:choice>
      <xs:any namespace='##other'/>
    </xs:choice>
  </xs:complexType>

  <xs:complexType name='specific'>
    <xs:sequence minOccurs='0'>
      <xs:any namespace='##other'/>
    </xs:sequence>
  </xs:complexType>

</xs:schema>
*/
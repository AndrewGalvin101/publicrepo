$(document).ready(function () {

  $('#get-weather-button').click(function (event) {

    $('#city').empty().attr({class: ''});
    $('#icon').empty();
    $('#description').empty();
    $('#conditions').empty();
    $('#three-hour-click').show();
    $('three-hour-forecasts').hide();
    $('#three-hour-head').empty();
    $('#three-hour-head-text').empty();
    $('#threeHour1').empty();
    $('#threeHour2').empty();
    $('#threeHour3').empty();
    $('#threeHour4').empty();
    $('#threeHour5').empty();
    //$('#forecast').empty();


      var zipCode = $('#zip-code').val();
      var zipCodeString = zipCode.toString();

    if (zipCodeString.length != 5 || zipCode > 99999) {
      $('#city').append("Error: Zip Code must be 5 digits!").attr({class: 'list-group-item list-group-item-danger'});
        $('#current').show();
        $('#forecast').hide();
      return false;
    }

    var units = $('#units').val();


    $.ajax({
        type: 'GET',
        url: 'https://api.openweathermap.org/data/2.5/weather?zip=' + zipCode + ',us&units=' + units + '&APPID=d9e71a33ab91bc85d6babe9fdcbe1b5d',
        success: function(data, status) {
          //  $('#errorMessages').empty()
         //  Clear the form and reload the table

          showCurrent(data, units);
          $('#footer').show();
        },
        error: function() {
            // $('#errorMessages')
            //    .append($('<li>')
            //    .attr({class: 'list-group-item list-group-item-danger'})
            //    .text('Error calling web service.  Please try again later.'));
        }

      });


      $.ajax({
        type: 'GET',
        url: 'https://api.openweathermap.org/data/2.5/forecast?zip=' +zipCode + ',us&units=' + units + '&APPID=d9e71a33ab91bc85d6babe9fdcbe1b5d&cnt=40',

      success: function(data) {
        console.log("Recieved data:",data);
        var weatherObj = [];
        var tempObj = [];
        $('#forehead').empty();
        $('#foretext').empty();
        $('#day0').empty();
        $('#day1').empty();
        $('#day2').empty();
        $('#day3').empty();
        $('#day4').empty();
        $('#forehead').append(data.city.name + " forecast ");
        $('#foretext').append("for <b> 24-hour periods</b>:");

        var todaysHighTemp = todaysLowTemp = 0;

        var timeUtc = data.list[0].dt;
        dateObj = new Date(timeUtc * 1000);
        day = dateObj.getDay();
        let localTime = moment(dateObj).format('h a');
        var j=0;
        var k=0;
        for (let dayOh = 1; dayOh <=5; dayOh++) {
          for (var i=j; i<j+8; i++) {
             tempObj.push(data.list[i].main.temp);
          }
          var todaysHighTemp = tempObj.reduce(function(a, b) {
            return Math.max(a, b);
          });
          k = j + tempObj.indexOf(todaysHighTemp);
          var todaysLowTemp = tempObj.reduce(function(a, b) {
            return Math.min(a, b);
          });
          tempObj = [];
          j=i;
          var dailyForecast = "";

          if (day==7) day=0;
          if (day==0) dayOfWeek = "Sunday";
          else if (day==1) dayOfWeek = "Monday";
          else if (day==2) dayOfWeek = "Tuesday";
          else if (day==3) dayOfWeek = "Wednesday";
          else if (day==4) dayOfWeek = "Thursday";
          else if (day==5) dayOfWeek = "Friday";
          else if (day==6) dayOfWeek= "Saturday";
          day++;
          if (day==7) day=0;
          if (day==0) nextDayOfWeek = "Sunday";
          else if (day==1) nextDayOfWeek = "Monday";
          else if (day==2) nextDayOfWeek = "Tuesday";
          else if (day==3) nextDayOfWeek = "Wednesday";
          else if (day==4) nextDayOfWeek = "Thursday";
          else if (day==5) nextDayOfWeek = "Friday";
          else if (day==6) nextDayOfWeek= "Saturday";

          dailyForecast += "<p>from <b> " + dayOfWeek + "</b> " + localTime + "</p>";
          dailyForecast += "<p>to <b> " + nextDayOfWeek + "</b> " + localTime + "</p>";
          dailyForecast += "<p>H <b>" + todaysHighTemp; // Temperature
          if (units=="metric") dailyForecast += "</b>&degC";
          else dailyForecast += "</b>&degF";
          dailyForecast += " | L<b> " + todaysLowTemp; // Temperature
          if (units=="metric") dailyForecast += "</b>&degC</p>";
          else dailyForecast += "</b>&degF</p>";
          dailyForecast += "<span> " + data.list[k].weather[0].description + "</span></p>"; // Description
          dailyForecast += "<p><img src='https://openweathermap.org/img/w/" + data.list[k].weather[0].icon + ".png'>" // Icon
          dailyForecast += "</p>" // Closing paragraph tag
          weatherObj.push(dailyForecast);
        }
        showForecast(weatherObj);
       }

      });

    });

//  });

  $('#three-hour-button').click(function (event) {
    $('#threeHour1').empty();
    $('#threeHour2').empty();
    $('#threeHour3').empty();
    $('#threeHour4').empty();
    $('#threeHour5').empty();

      zipCode = $('#zip-code').val();
      zipCodeString = zipCode.toString();

      if (zipCodeString.length != 5 || zipCode > 99999)  {
      $('#current').hide();
      $('#city').append("Error: Zip Code must be 5 digits!").attr({class: 'list-group-item list-group-item-danger'});
      $('#current').show();
      return false;
    }

    units = $('#units').val();

    $.ajax({
      type: 'GET',
      url: 'https://api.openweathermap.org/data/2.5/forecast?zip=' +zipCode + ',us&units=' + units + '&APPID=d9e71a33ab91bc85d6babe9fdcbe1b5d&cnt=40',

      success: function(data) {
        p=0;
        for (let dayOh = 1; dayOh <=5; dayOh++) {
        //  thr3hourForecast = "<p><b>"
          thr3hourForecast = "";
          // if (dayOh == 1) thr3hourForecast += "Today</b>:</p>";
          // else if (dayOh == 2) thr3hourForecast += "Tomorrow</b>:</p>";
          // else thr3hourForecast += "Day " + dayOh + "</b>:</p>";
          // thr3hourForecast += "<hr />";
          bump = '#threeHour' + dayOh;
          for (var n=p; n<p+8; n++) {
            timeUtc = data.list[n].dt;
            dateObj = new Date(timeUtc * 1000);
            day = dateObj.getDay();
            let localTime = moment(dateObj).format('h a');
            if (day==0) dayOfWeek = "Sunday";
            else if (day==1) dayOfWeek = "Monday";
            else if (day==2) dayOfWeek = "Tuesday";
            else if (day==3) dayOfWeek = "Wednesday";
            else if (day==4) dayOfWeek = "Thursday";
            else if (day==5) dayOfWeek = "Friday";
            else if (day==6) dayOfWeek= "Saturday";
        //    let dateObj2 = new Date((timeUtc * 1000) + (3 * 60 * 60 * 1000))
        //    let endTime = moment(dateObj2).format('h a');
            var d = new Date();
            var today = d.getDay();
            if (day == today) thr3hourForecast+= "<p><b>  Today</b><p>";
            else thr3hourForecast+= "<p><b> " + dayOfWeek + "</b></p>";
            thr3hourForecast+= "<p> " + localTime + "</p>";
            thr3hourForecast+= "<p> Temp: " + data.list[n].main.temp;
            if (units=="metric") thr3hourForecast += "&degC</p>";
            else thr3hourForecast += "&degF</p>";
            thr3hourForecast+= "<p><span> " + data.list[n].weather[0].description + "</span></p>"
            thr3hourForecast+= "<p><img src='https://openweathermap.org/img/w/" + data.list[n].weather[0].icon + ".png'>"
            thr3hourForecast+= "<p></p><hr />";
          }
          $(bump).empty();
          $(bump).append(thr3hourForecast);
          p=n;
        }
        $('#three-hour-forecasts').show();
        $('#three-hour-head').append(data.city.name + " forecast ");
        $('#three-hour-head-text').append("in <b> 3-hour increments</b>:");
        $('#three-hour-click').hide();
      },
    });
  });

});

function showCurrent(data, units) {
  // $('#errorMessages').empty();
  $('#city').append(data.name + " now");
  var path = 'http://openweathermap.org/img/w/' + data.weather[0].icon + ".png";
  $('#icon').append('<br /> <img src="' + path + '" alt=icon"> <h7><b>' + data.weather[0].description + '</b></h7>');
  $('#description').append(" " + data.weather[0].description);
  var temp = data.main.temp;
  var wind = data.wind.speed;
  var humidity = data.main.humidity;
  if (units == 'imperial') {
    $('#conditions').append('<p><b>Temperature: </b>' + temp  + '&degF</p><p><b>Humidity: </b>' + humidity + '%</p><p><b>Wind: </b>' + wind + ' mph</p>')
  }
  else {
    $('#conditions').append('<p><b>Temperature: </b>' + temp  + '&degC</p><p><b>Humidity: </b>' + humidity + '%</p><p><b>Wind: </b>' + wind + ' m/s</p>')
  }
  $('#current').show();
}

function showForecast(weatherObj) {
  var day ="";
  for (let m=0; m<5; m++) {
    var dailyForecast = weatherObj[m];
    day = "#day" + (m+1);
    $(day).empty();
    $(day).append(dailyForecast);
  }
  $('#forecast').show();
}

// q=19803
// &APPID=d9e71a33ab91bc85d6babe9fdcbe1b5d
// data.weather[description]

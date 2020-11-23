import './snow.html';

var MAX_SPEED = 5;
var MIN_SPEED = 10;

var MAX_PATH_WIDTH = 20;
var MIN_PATH_WIDTH = 50;

var MAX_PATH_HEIGHT = getDocumentHeight();
var MIN_PATH_HEIGHT = 100;
var NUM_OBJECTS = 50;

var MAX_SIZE = 32;
var MIN_SIZE = 20;

var snow = [];

/**
 * This function is a cross-browser function that gets the document height.
 * @return Document height if able to find it, otherwise -1.
 * @see http://www.howtocreate.co.uk/tutorials/javascript/browserwindow
 */
function getDocumentHeight()
{
  if(typeof(window.innerWidth) == 'number')
  {
    //Non-IE
    return window.innerHeight;
  }
  else if(document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight))
  {
    //IE 6+ in 'standards compliant mode'
    return document.documentElement.clientHeight;
  }
  else if( document.body && (document.body.clientWidth || document.body.clientHeight) )
  {
    //IE 4 compatible
    return document.body.clientHeight;
  }
  else
  {
    // Unable to find height
    return -1;
  }
}

/**
 * This function is a cross-browser function that gets the document width.
 * @return Document width if able to find it, otherwise -1.
 * @see http://www.howtocreate.co.uk/tutorials/javascript/browserwindow
 */
function getDocumentWidth()
{
  if(typeof(window.innerWidth) == 'number')
  {
    //Non-IE
    return window.innerWidth;
  }
  else if(document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight))
  {
    //IE 6+ in 'standards compliant mode'
    return document.documentElement.clientWidth;
  }
  else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) )
  {
    //IE 4 compatible
    return document.body.clientWidth;
  }
  else
  {
    // Unable to find wodth
    return -1;
  }
}

/**
 * Returns a random number between min and max.
 * @param number min The lower bound of the range.
 * @param number max The upper bound of the range.
 * @return number A random number between min and max.
 */
function random(min, max)
{
  return Math.random() * (max - min) + min;
}

function sinGraph(value, height, waveLength)
{
  return height * Math.sin((2 * Math.PI / waveLength) * value);
}

/**
 * Create a new snow flake object in the specified starting position
 * @param Image imageObj The image object to be used as a snow flake
 */
function SnowFlake(imageObj)
{
  var that = this;

  this.imageObj = imageObj;
  this.interval = null;


  this._reset();
}

/**
 * Resets teh status of the object with new random values
 */
SnowFlake.prototype._reset = function() {
  var size;

  this.startX = random(0, getDocumentWidth());
  this.startY = -1 * random(0, getDocumentHeight());
  this.x = this.startX;
  this.y = this.startY;

  this.speed = random(MIN_SPEED, MAX_SPEED);
  this.pathWidth = random(MIN_PATH_WIDTH, MAX_PATH_WIDTH);
  this.pathHeight = random(MIN_PATH_HEIGHT, MAX_PATH_HEIGHT);

  size = random(MIN_SIZE, MAX_SIZE);
  this.imageObj.width = size;
  this.imageObj.height = size;
};

/**
 * Starts an infinite animation loop using the given function to move and change the size of the given object.
 */
SnowFlake.prototype._animation = function (funcMoveX, funcSizeWidth) {
  this.y += this.speed;

  if(this.pathWidth === 0 || this.pathHeight === 0)
  {
    this.x = funcMoveX(this.y) + this.startX;
  }
  else
  {
    this.x = funcMoveX(this.y, this.pathWidth, this.pathHeight) + this.startX;
  }

  // check if snow flake y value is out of the frame
  if(this.y >= window.innerHeight)
  {
    this._reset();
  }
  else
  {
    this.imageObj.style.top = parseInt(this.y, 10) + "px";
  }

  if(this.x <= window.innerWidth)
  {
    this.imageObj.style.left = parseInt(this.x, 10) + "px";
  }
};

/**
 * Starts the animation for this object. To stop the animation call stopAnimation.
 */
SnowFlake.prototype.startAnimation = function()
{
  var that = this;
  this.interval = setInterval(function(){ that._animation(sinGraph, null); }, 100);
};

/**
 * Stops the animation for this object. To start the animation again call startAnimation.
 */
SnowFlake.prototype.stopAnimation = function(){
  clearInterval(this.interval);
};

export default function initAnimation()
{
  var object;
  var newElementId;
  var html = "";
  var i;

  // add snow flakes images to the html
  for(i = 0; i < NUM_OBJECTS; i++)
  {
    newElementId = "snow" + i;
    html += "<img id=\"" + newElementId + "\"src=\"snowflake.png\" width=\"32\" height=\"32\" style=\"position: absolute;\" />";
  }

  document.body.innerHTML += html;

  // initialize the animation for the snow flakes
  for(i = 0; i < NUM_OBJECTS; i++)
  {
    object = document.getElementById("snow" + i);
    snow.push(new SnowFlake(object));
    snow[i].startAnimation();
  }
}

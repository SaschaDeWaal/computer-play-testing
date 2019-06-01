import React, {Component} from 'react';
import {withStyles} from '@material-ui/core/styles';

const styles = (theme) => ({
  container: {
    margin: '20px',
    border: '1px solid black',
    borderRadius: '10px',
    overflow: 'hidden',
  },
  block: {
    width: '100px',
    height: '20px',
    padding: '5px',
    color: '#000',
    textAlign: 'left',
    fontSize: '12px',
    borderBottom: '1px solid #b5b5b5',
    borderLeft: '1px solid #b5b5b5',
    borderRight: '1px solid #b5b5b5',
  },
  indexBlock: {
    width: '100px',
    height: '20px',
    padding: '5px',
    color: '#000',
    textAlign: 'left',
    borderBottom: '1px solid #b5b5b5',
    borderLeft: '1px solid #b5b5b5',
  },
  bar: {
    width: '110px',
    display: 'inline-block',
    textAlign: 'center'
  },
  title: {
    whiteSpace: 'pre-line',
    borderBottom: '1px solid #b5b5b5',
    borderLeft: '1px solid #b5b5b5',
  },
  popup: {
    position: 'relative',
    left: '110px',
    top: '10px',
    width: '200px',
    height: '50px',
    background: '#000',
    color: '#fff',
    zIndex: '1000'
  }
});

Array.prototype.sum = function(selector) {
  if (typeof selector !== 'function') {
    selector = function(item) {
      return item;
    }
  }
  let sum = 0;
  for (let i = 0; i < this.length; i++) {
    sum += parseFloat(selector(this[i]));
  }
  return sum;
};

class HeatMapBalk extends Component {


  constructor(props) {
    super(props);

    this.state = {
      selectX: -1,
      selectY: -1,
    }
  }

  createColor(delta) {

    if (delta === 0) {
      return {r: 200, g: 200, b: 200, a: 255};
    }

    if (delta < 0.5) {
      const subDelta = delta * 2; // 0 - 1
      return { r: (subDelta * 255), g: 255, b: 0, a: 255}

    }else {
      const subDelta = ((delta - 0.5) * 2); // 0 - 1
      return { r: 255, g: 255 - (subDelta * 255), b: 0, a: 255}
    }

  }

  render() {

    const {classes, names, data, labels} = this.props;
    const {selectX, selectY } = this.state;

    let maxSteps = 0;
    let barData = [];

    names.forEach((name, nameIndex) => {
      let result = {};
      let points = [];
      let maxFate = 0;
      let biggestAmount = 0;

      // get amount of numbers
      data[name].forEach(d => {
        const p = d.fatePoints.sum();


        result[p] = ( result[p]) ? result[p] + 1.0 : 1.0;
        maxFate = (p > maxFate) ? p : maxFate;
        biggestAmount = (result[p] > biggestAmount) ? result[p] : biggestAmount;


      });

      // add blocks
      for(let i = maxFate; i >= 0; i--) {
        let amount = (result[i]) ? (result[i] / biggestAmount) : 0;
        let color = this.createColor(amount);

        points.push({
          index: i, color: color, amount: (result[i]) ? result[i] : 0,
        });
      }


        points.push({
          index: 0,
          color: {r: 255, g: 255, b: 0},
          amount: data[name].length,
        });

      maxSteps = (maxFate > maxSteps) ? maxFate : maxSteps;

      barData.push({
        points: points,
        name: labels[nameIndex],
      });

    });

    // make nicer
    barData.forEach(b => {
      for(let i = b.points.length; i <=  maxSteps + 1; i++) {
        b.points.splice(0, 0, {index: i, color: this.createColor(0), amount: 0});
      }
    });

    // Create index
    const indexList = [];
    for(let i = maxSteps; i >= -1; i--) {
      const label = (i === -1) ? 'Total played' : `${i} Fate`;
      indexList.push(<div className={classes.indexBlock} key={i} style={{background:  (maxSteps - i === selectY ? `#aaa` : `#fff`)}}>{label}</div>)
    }

    return <div className={classes.container} style={{width: `${(barData.length + 1) * 110}px`}} onMouseLeave={() => this.setState({selectX: -1, selectY: -1})}>
      <div className={classes.bar}>
        {indexList}
        <div className={classes.title}> Cards </div>
      </div>
      {barData.map((bar, x) => <div className={classes.bar} key={bar.name}>
        {bar.points.map((d, y) =>
          <div className={classes.block}
               style={{background: `rgba(${d.color.r}, ${d.color.g}, ${d.color.b}, ${d.color.a})`, filter:  ((x === selectX && y >= selectY) || (y === selectY && x <= selectX)) ? `brightness(90%)` : ``}}
               key={`${bar.name}${d.index}${d.amount}${Math.random()}`}
               onMouseOver={() => this.setState({selectX: x, selectY: y})}>
            {(d.amount === 0) ? '' : `${d.amount} times`}
            {(selectX === x && selectY === y && d.amount > 0 && false) ? <div className={classes.popup}>
              {bar.name}

            </div> : ''}
        </div>)}
        <div className={classes.title} style={{background:  (x === selectX ? `#aaa` : `#fff`)}}>
          {bar.name}
        </div>
        </div>)}

    </div>
  }
}

export default withStyles(styles)(HeatMapBalk);

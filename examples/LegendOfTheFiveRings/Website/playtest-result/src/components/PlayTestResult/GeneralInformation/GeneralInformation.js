import React, { Component } from 'react';
import { Paper, Typography } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';
import { LegendOrdinal } from '@vx/legend';

import { RadialChart, ArcSeries, ArcLabel } from '@data-ui/radial-chart';

import HeatMapBalk from '../HeatmapBalk/HeatMapBalk';

const styles = (theme) => ({
  root: {
  },
  row: {
    cursor: 'pointer'
  },
  dataSubject: {
    padding: theme.spacing.unit * 2,
    marginBottom: theme.spacing.unit * 2,
  },
  chartContent: {
    display: 'flex',
    flexWrap: 'wrap',
    alignItems: 'center',
    textAlign: 'center',
  },
  chart: {
    background: '#ececec',
    margin: theme.spacing.unit * 2,
    minHeight: '550px',
  },
  chartLabel: {
    padding: theme.spacing.unit * 2
  },
  legend: {
    padding: theme.spacing.unit * 2,
    width: '200px',
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

class GeneralInformation extends Component {


  toReadable(name) {
    return name.replace('DynastyCharacter', '');
  }

  generateBattleData() {
    const {  battles } = this.props;

    const cards = {};

    battles.forEach(b => {

      if (b.cardName.length === 0) {
        return;
      }

      if (cards[b.cardName] === undefined) {
        cards[b.cardName] = {
          battles: [],
          wonTimes: 0,
          battled: 0,
          political: {
            wonTimes: 0,
            played: 0
          },
          military: {
            wonTimes: 0,
            played: 0
          }

        };
      }

      cards[b.cardName].battles.push(b.battles);
      cards[b.cardName].wonTimes += (b.battles.wonBattle) ? 1 : 0;
      cards[b.cardName].battled ++;

      cards[b.cardName][b.battles.typeBattle.toLowerCase()].wonTimes += (b.battles.wonBattle) ? 1 : 0;
      cards[b.cardName][b.battles.typeBattle.toLowerCase()].played ++;
    });

    const keys = Object.keys(cards).sort();

    return {
      all: keys.map(k => ({label: this.toReadable(k), value: (cards[k].wonTimes / cards[k].battled) * 100, played: cards[k].battled, won: cards[k].wonTimes })),
      political: keys.map(k => ({label: this.toReadable(k), value: (cards[k].political.wonTimes / cards[k].political.played) * 100, played: cards[k].political.played, won: cards[k].political.wonTimes })),
      military: keys.map(k => ({label: this.toReadable(k), value: (cards[k].military.wonTimes / cards[k].military.played) * 100, played: cards[k].military.played, won: cards[k].military.wonTimes}))
    };

  }

  drawRadialChart(data, label) {
    const { classes, colorScale } = this.props;

    return (<div className={classes.chart} key={label}>
      <Typography variant="h5" gutterBottom className={classes.chartLabel}>
        {label}
      </Typography>
      <RadialChart
        ariaLabel={label}
        width={500}
        height={500}
        renderTooltip={({ event, datum, data, fraction }) => (
          <div>
            <strong>{datum.label}</strong><br />
            {(datum.value).toFixed(2)}% of battles won <br />
            {datum.won}/{datum.played}
          </div>
        )}
      >
        <ArcSeries
          data={data}
          pieValue={d => d.value}
          label={(e) => e.data.label}
          fill={arc => colorScale(arc.data.label)}
          stroke="#fff"
          strokeWidth={1}
          labelComponent={<ArcLabel />}
          innerRadius={radius => 0.35 * radius}
          outerRadius={radius => 0.6 * radius}
          labelRadius={radius => 0.75 * radius}
        />
      </RadialChart>
    </div>);
  }


  render() {
    const { classes, cardsNames, cardsData, colorScale, battles } = this.props;
    const readableNames = cardsNames.map(name => name.replace('DynastyCharacter', ''));

    const battleData = this.generateBattleData();

    if (cardsNames.length === 0) {
      return '';
    }

    return (
        <Paper className={classes.dataSubject}>
          <Typography variant="h5" gutterBottom>General</Typography>

          <div className={classes.chartContent}>
            <div className={classes.chart}>
              <Typography variant="h5" gutterBottom className={classes.chartLabel}>
                Cost table (heatmap)
              </Typography>
              <HeatMapBalk data={cardsData} names={cardsNames} labels={readableNames} />
            </div>

            {this.drawRadialChart(battleData.all, 'All conflicts win ratio')}
            {this.drawRadialChart(battleData.political, 'Political conflicts win ratio')}
            {this.drawRadialChart(battleData.military, 'Military conflicts win ratio')}

          </div>
          <div className={classes.chart}>
            <Typography variant="h5" gutterBottom className={classes.chartLabel}>
              Legend
            </Typography>
            <LegendOrdinal
              className={classes.legend}
              direction="column"
              scale={colorScale}
              shape="rect"
              fill={({ datum }) => colorScale(datum)}
              labelFormat={label => label}
            />
          </div>

        </Paper>
    );
  }
}

export default withStyles(styles)(GeneralInformation);

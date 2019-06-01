import React, { Component } from 'react';
import { Paper, Typography, Table, TableHead, TableRow, TableCell, TableBody, Button } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';

const styles = (theme) => ({
  root: {
    flexGrow: 1,
  },
  row: {
    cursor: 'pointer'
  },
  dataSubject: {
    padding: theme.spacing.unit * 2,
    marginBottom: theme.spacing.unit * 2,
  },
  rowTitle: {
    width: '400px',
  }
});

class CardInformation extends Component {

  logRawData() {
    const { data, battles, name } = this.props;

    console.log('battles', battles);
    console.log('data', data);
  }

  getBattleData() {
    const {  battles } = this.props;

    let battled = 0;
    let won = 0;

    const names = battles.reduce((s, e) => {
      e.battles.enemyCards.forEach(enemyName => {
        if (enemyName.length === 0) {
          enemyName = 'No enemy';
          return s;
        }

        if (s.filter(o => o.name === enemyName).length === 0) {
          s.push({name: enemyName, amount: 0, won: 0});
        }

        const item = s.find(i => i.name === enemyName);
        item.amount ++;
        battled++;
        if (e.battles.wonBattle) {
          item.won ++;
          won++;
        }

      });
      return s;
    }, []);

    return {
      enemies: (names.sort((a, b) => b.amount - a.amount).map(o => `${o.name.replace('DynastyCharacter', '')} (${o.won}/${o.amount})`).join(', ')),
      battled,
      won
    }
  }

  getAttachments() {
    const { data } = this.props;

    const attachments = data.reduce((l, c) => {
      c.attachments.forEach(a => {
        if(!l[a]) l[a] = 0;
        l[a]++;
      });

      return l;
    }, []);

    if (Object.keys(attachments).length === 0) {
      return 'none';
    }

    attachments.sort((a, b) => a - b);

    return Object.keys(attachments).map(k => `${k} (${attachments[k]})`).join(', ');
  }

  render() {
    const { classes, name, data, colorScale, index } = this.props;

    const readableName = name.replace('DynastyCharacter', '');
    const cost = data.reduce((s, e) => s + e.fatePoints.reduce((o, f) => o + f, 0), 0) / data.length;
    const battleData = this.getBattleData();

    return (
      <Paper className={classes.dataSubject}>
        <Typography variant="h5" gutterBottom style={{color: colorScale(readableName)}}>{index+1}. {readableName}</Typography>
        <Button onClick={() => this.logRawData()}>Dump raw data to console</Button>

        <Table className={classes.table}>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Amount</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            <TableRow>
              <TableCell component="th" scope="row" className={classes.rowTitle}>Full circle tested (to the end)</TableCell>
              <TableCell component="th" scope="row">{data.length}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell component="th" scope="row" className={classes.rowTitle}>Tested conflicts</TableCell>
              <TableCell component="th" scope="row">{battleData.battled}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell component="th" scope="row" className={classes.rowTitle}>Won battles</TableCell>
              <TableCell component="th" scope="row">{battleData.won}/{battleData.battled}  ({(battleData.won / battleData.battled * 100).toFixed(2)}%)</TableCell>
            </TableRow>
            <TableRow>
              <TableCell component="th" scope="row" className={classes.rowTitle}>Battled Enemies</TableCell>
              <TableCell component="th" scope="row">{battleData.enemies}</TableCell>
            </TableRow>
            <TableRow>
              <TableCell component="th" scope="row" className={classes.rowTitle}>Avereage cost (incl attachments)</TableCell>
              <TableCell component="th" scope="row">{cost.toFixed(2)} Fate</TableCell>
            </TableRow>
            <TableRow>
              <TableCell component="th" scope="row" className={classes.rowTitle}>Attachment</TableCell>
              <TableCell component="th" scope="row">{this.getAttachments()}</TableCell>
            </TableRow>
          </TableBody>
        </Table>

      </Paper>
    );
  }
}

export default withStyles(styles)(CardInformation);

import React, { Component } from 'react';
import { Paper, Table, TableHead, TableRow, TableCell, TableBody, Button } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash, faSync } from "@fortawesome/free-solid-svg-icons/index";

const styles = (theme) => ({
  root: {
    flexGrow: 1,
  },
  row: {
    cursor: 'pointer'
  },
  buttonContainer: {
    padding: theme.spacing.unit * 2,
  }
});


class PlaytestList extends Component {

  render() {
    const { classes, list, onSelect, onDelete, onReload } = this.props;

    return (
      <Paper className={classes.root}>
        <Table className={classes.table}>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Action</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {list.map(item => (
              <TableRow key={item} className={classes.row}>
                <TableCell component="th" scope="row" onClick={() => onSelect(item)}>{item.replace('play_', '').replace(/_/g," ")}</TableCell>
                <TableCell component="th" scope="row"><Button onClick={() => onDelete(item)}><FontAwesomeIcon icon={faTrash} /></Button> </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
        <div className={classes.buttonContainer}>
         <Button variant="contained" color="primary" onClick={() => onReload()}><FontAwesomeIcon icon={faSync}/></Button>
        </div>
      </Paper>
    );
  }
}

export default withStyles(styles)(PlaytestList);

import React, { Component } from 'react';
import { AppBar, Toolbar, Typography } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';

import NodeCouchDb from 'node-couchdb';
import config from '../config';

import PlaytestList from './PlaytestList';
import TestResult from './PlayTestResult/TestResult';

import '../App.css';

const couch = new NodeCouchDb(config.couchDb);

const styles = (theme) => ({
  root: {
    flexGrow: 1,
  },
  content: {
    margin: theme.spacing.unit * 4
  }
});

class App extends Component {

  constructor(props) {
    super(props);

    const hash = window.location.hash.replace('#', '');

    this.state = {
      databases: [],
      selected: (hash.length > 0) ? hash : null,
    };
  }

  componentDidMount() {
    this.loadDatabases();
  }

  loadDatabases() {
    // Get all databases
    couch.listDatabases().then(dbs => {
      this.setState({databases: dbs.filter(d => d.startsWith('play_'))});
    }, err => console.error(err));

  }

  selectPlay(id) {
    this.setState({selected: id});

    window.location = `/#${id}`;
  }

  remove(id) {
    if (window.confirm(`Are you sure you want to delete ${id}?`)) {
      couch.dropDatabase(id).then(() => {
        this.loadDatabases();
      });
    }
  }

  onClose() {
    this.setState({selected: null});
  }



  render() {

    const { classes } = this.props;
    const { databases, selected } = this.state;

    return (
      <div className={classes.root}>
        <AppBar position="static">
          <Toolbar>
            <Typography variant="h6" color="inherit">
              Playtest results
            </Typography>
          </Toolbar>
        </AppBar>
        <div className={classes.content}>
          <PlaytestList list={databases} onSelect={(id) => this.selectPlay(id)} onDelete={(id) => this.remove(id)} onReload={() => this.loadDatabases()}/>
          <TestResult playtestID={selected} onClose={() => this.onClose()} couch={couch}/>
        </div>
      </div>
    );
  }
}

export default withStyles(styles)(App);

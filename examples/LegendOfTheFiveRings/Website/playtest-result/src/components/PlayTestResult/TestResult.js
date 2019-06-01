import React, {Component} from 'react';
import { Dialog, AppBar, Toolbar, IconButton, Typography, Slide } from '@material-ui/core';
import  {withStyles } from '@material-ui/core/styles';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTimes, faSync, faCheck, faSquare, faPlay, faPause } from '@fortawesome/free-solid-svg-icons';

import { scaleOrdinal } from '@vx/scale';
import { color as colors } from '@data-ui/theme';

import GeneralInformation from './GeneralInformation/GeneralInformation';
import CardInformation from './CardInformation/CardInformation';

const styles = (theme) => ({
  root: {

  }, row: {
    cursor: 'pointer'
  }, dataContainer: {
    margin: theme.spacing.unit * 4, marginTop: theme.spacing.unit * 10,
  }, dataSubject: {
    padding: theme.spacing.unit * 2, marginBottom: theme.spacing.unit * 2,
  }
});

const colorScale = scaleOrdinal({ range: colors.categories });

let timer = null;


function Transition(props) {
  return <Slide direction="up" {...props} />;
}

class TestResult extends Component {

  constructor(props) {
    super(props);

    this.state = {
      timeline: [], cardsNames: [], cardsData: {}, battles: [], loading: true, refresh: false,
    }
  }

  componentDidMount() {
    timer = setInterval(() => {
      if (this.state.refresh) {
        console.log('refresh');
        const {couch, playtestID} = this.props;
        this.loadData(couch, playtestID, false);
      }
    }, 30 * 1000);
  }

  componentWillUnmount() {
    clearInterval(timer);
  }


  componentWillReceiveProps(nextProps) {
    const {couch, playtestID} = nextProps;

    if (playtestID === null) {
      return;
    }

    this.loadData(couch, playtestID, true)
  }

  close() {
    const {onClose} = this.props;

    this.setState({
      cardsNames: [], cardsData: [],
    });

    onClose();
  }

  loadData(couch, playtestID, showLoading) {

    if (showLoading) {
      this.setState({
        cardsNames: [], cardsData: [], battles: [], loading: true
      });
    }

    // load cards
    couch.mango(playtestID, {
      selector: {'ResultType': 'Character'},
      limit: 900000
    }, {}).then(({data, headers, status}) => {
      let cardsNames = [];
      let cardsData = {};

      data.docs.forEach(e => {
        if (cardsNames.includes(e.cardName) === false) {
          cardsNames.push(e.cardName);
        }

        if (!cardsData[e.cardName]) cardsData[e.cardName] = [];
        cardsData[e.cardName].push(e);

      });

      cardsNames = cardsNames.sort();

      this.setState({
        cardsNames, cardsData, loading: false
      });

    });

    // load battles
    couch.mango(playtestID, {
      selector: {'ResultType': 'Battle'},
      limit: 900000
    }, {}).then(({data, headers, status}) => {
      this.setState({
        battles: data.docs
      });

    });
  }

  render() {
    const {classes, playtestID, couch} = this.props;
    const {cardsData, cardsNames, battles, loading, refresh} = this.state;

    return (<Dialog fullScreen open={playtestID !== null} onClose={() => this.close()} TransitionComponent={Transition}>
        <AppBar className={classes.appBar}>
          <Toolbar>
            <IconButton color="inherit" onClick={() => this.close()} aria-label="Close">
              <FontAwesomeIcon icon={faTimes} />
            </IconButton>
            <IconButton color="inherit" onClick={() => this.loadData(couch, playtestID, true)} aria-label="Close">
              <FontAwesomeIcon icon={faSync} />
            </IconButton>
            <IconButton color="inherit" onClick={() => this.setState({refresh : !refresh})} aria-label="Close">
              <FontAwesomeIcon icon={(refresh) ? faPause : faPlay} />
            </IconButton>
            <Typography variant="h6" color="inherit" className={classes.flex}>
              {(playtestID) ? playtestID.replace(/_/g," ") : ''}
            </Typography>
          </Toolbar>
        </AppBar>
        <div className={classes.dataContainer}>
          {cardsNames.length === 0 && loading ? 'Loading and generating...' : ''}
          {cardsNames.length === 0 && !loading ? 'Nothing to show yet' : ''}
          <GeneralInformation cardsNames={cardsNames} cardsData={cardsData} colorScale={colorScale} battles={battles}/>
          {cardsNames.map((e, i) => <CardInformation key={e} name={e} data={cardsData[e]} colorScale={colorScale}  battles={battles.filter(b => b.cardName === e)} index={i}/>)}
        </div>
      </Dialog>);
  }
}

export default withStyles(styles)(TestResult);

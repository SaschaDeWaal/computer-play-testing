const net = require('net');
const NodeCouchDb = require('node-couchdb');
const config = require('./config');

const couch = new NodeCouchDb(config.couchDb);

const Insert = (json, databaseName) => {
  try {
    couch.insert(databaseName, json).then(() => {
    }, (e) => console.error(e));
  }catch (e) {

  }
};

const server = net.createServer((socket) => {
  let databaseName = '';

  socket.on('close', () => {
    console.log('Connection is closed. All data is saved in ' + databaseName);
  });

  socket.on('data', (chunk) => {
    const data = chunk.toString('utf8').split(';');

    data.forEach(d => {
      if (d.length === 0) {
        return;
      }

      try {
        const json = JSON.parse(d);

        switch (json.action) {
          case 'startTest':
            databaseName = `play_${json.data.id.toLowerCase().replace(/[^a-z0-9 ]/g, "").replace(/ /g,"_")}`;
           couch.listDatabases().then(dbs => {
              if (dbs.includes(databaseName) === false) {
                couch.createDatabase(databaseName);
                console.log(`Created database ${databaseName}`)
              } else {
                console.log(`Will use database ${databaseName}`)
              }
            });
            break;

          case 'insert':
            Insert(json.data, databaseName, true);
            break;
        }

      } catch (e) {

      }

    });
  });
});

server.listen(config.server.port);
console.log(`running server on port ` + config.server.port);

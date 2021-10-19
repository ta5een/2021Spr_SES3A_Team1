const MongoClient = require('mongodb').MongoClient;
const ObjectID = require('mongodb').ObjectId;
const functions = require("firebase-functions");
const dbClient = new MongoClient(`mongodb+srv://infiniracer:${functions.config().infiniracer.dbpass}@infiniracer.ifeau.mongodb.net/InfiniRacer?retryWrites=true&w=majority`, {useNewURLParser: true, useUnifiedTopology: true});

// Create attention session
exports.CreateAttentionSession = functions.region('australia-southeast1').https.onRequest(async (req,res) => {
    const name = req.body.name || 'Anonymous';
    const attentionBefore = req.body.attentionBefore;
    const attentionAfter = req.body.attentionAfter;
    const playTime = req.body.playTime;
    const score = req.body.score || 0;
    try {
        await dbClient.connect();
        const db = dbClient.db('InfiniRacer');
        const attentionSessions = db.collection("AttentionSessions");

        const recordData = {
            name,
            attentionBefore,
            attentionAfter,
            playTime,
            score
        }

        await attentionSessions.insertOne(recordData);

        return res.send('Created attention session successfully!')
    } catch (err) {
        return res.send(err);
    }
});

// Read attention session by id
exports.ReadAttentionSession = functions.region('australia-southeast1').https.onRequest(async (req,res) => {
    const sessionID = req.body.sessionID;
    try {
        await dbClient.connect();
        const db = dbClient.db('InfiniRacer');
        const attentionSessions = db.collection("AttentionSessions");

        const targetSession = await attentionSessions.findOne({_id: new ObjectID(sessionID)});

        return res.status(200).send(targetSession);
    } catch (err) {
        return res.send(err);
    }
});

// Get all attention sessions
exports.ReadAllAttentionSession = functions.region('australia-southeast1').https.onRequest(async (req,res) => {
    try {
        await dbClient.connect();
        const db = dbClient.db('InfiniRacer');
        const attentionSessions = db.collection("AttentionSessions");

        const allSessions = await attentionSessions.find().toArray();

        return res.status(200).send({message: 'Found all sessions successfully!', sessions: allSessions});
    } catch (err) {
        return res.send(err);
    }
});

// Add new user score (usually triggered at the end of a playthrough session)
exports.AddNewScore = functions.region('australia-southeast1').https.onRequest(async (req,res) => {
    const name = req.body.name || 'Anonymous';
    const score = req.body.score || 0;
    try {
        await dbClient.connect();
        const db = dbClient.db('InfiniRacer');
        const highScores = db.collection("HighScores");

        await highScores.insertOne({
            name,
            score
        });

        return res.status(200).send("Added new high score!");
    } catch (err) {
        return res.send(err);
    }
});

// Get top 5 scores from the DB to show as the public high score table in game
exports.GetTopScores = functions.region('australia-southeast1').https.onRequest(async (req,res) => {
    try {
        await dbClient.connect();
        const db = dbClient.db('InfiniRacer');
        const highScores = db.collection("HighScores");

        // Get top 5 highest scores
        const topFiveScores = await highScores.find().sort({score: -1}).limit(5).toArray();

        return res.status(200).send({message: "Successfully found top 5 recorded high scores", topFiveScores});
    } catch (err) {
        return res.send(err);
    }
});
using TinderImport;

// Test 01
var tinderOperations = new TinderOperations(null);
//var accountId = "63b3446307fbca0100fd9d13"; //2023-01-02_kamil_63b3446307fbca0100fd9d13
var kamilStareId = "635e47ac1e983b01004469ac"; //2022-10-30_kamil_635e47ac1e983b01004469ac
var previousIds = tinderOperations.GetAllAccountMatchesIds(kamilStareId);

//var googleDocId = "1IMXLO5iihnDnFZIJjCY6yrj3kUqaX8sjuRk0ZsU1v98";
//var accountId = "63accf430996fb0100a34a72";
var personId = "63af3ff7cc4e210100fe94fa";

//foreach (var id in previousIds)
//{
//    Console.WriteLine('\"' + id + "\",");
//}

var previousIdsList = new List<string>()
{
    "604fbb095b7d3e0100e46cb0",

};

//tinderOperations.ExportMatchToGoogleDoc(googleDocId, accountId, personId);

var personIdQBluprintList = new List<(string, string)>()
{
    ("604fbb095b7d3e0100e46cb0", "a"),
    ("604fbb095b7d3e0100e46cb0", "a"),
    ("59fdc18f9e21afaa5483804e", "p"),
    ("5af316512d8e6661651a1c01", "z/s"),
    ("5b200a592e4f766465796887", "s"),
    ("5dc9b596cff0420100cf1f61", "p"),
    ("5e41e6f3577f75010094d1bc", "p/z"),
    ("5e82614d20414d0100120f49", "z"),
    ("6020e893e8fe580100c1029d", "szc"),
    ("604b73f4ea21880100fcd2e5", "z/s"),
    ("6053bcabae07aa01001671e7", "?"),
    ("606ce2c409b8660100a8c092", "?"),
    ("60b5543402c5d40100a7ee58", "s"),
    ("6169ee022024190100649917", "szc"),
    ("61807c9ae9a4b60100a5501e", "szc"),
    ("61f4887e871bc601003896a7", "a"),
    ("621935d52b2470010064de42", "szc/z"),
    ("62855bf17516940100788a91", "a/v"),
    ("629b19ba612fab01006d6991", "p/s"),
    ("62b4bb078ee48f010045ce19", "szc"),
    ("62d51395cc7a2b010068a685", "v"),
    ("62fa6357c27c2b0100692cfa", "v"),
    ("62ff2a2135f6540100eccc32", "szc"),
    ("6318c3d11d6f82010018e7d1", "?"),
    ("63251fd88b625f0100face5a", "?"),
    ("633d6b73f7154201001ea8ba", "s"),
    ("6345cb34f715420100215a26", "p"),
    ("6345fd30ff701201007f1662", "?"),
    ("634718e32cc9590100260a3e", "p"),
    ("634959b3c499f6010097b957", "v"),
    ("636821b3c4281e01002c4f34", "x"),
    ("636971af18d4b20100b9d22c", "?"),
    ("637002118c9150010053be96", "?"),
    ("6370177e1e983b01004b54cc", "?"),
    ("6372dddd1d5d650100f39409", "szc"),
    ("63753e5b23d831010007f409", "v"),
    ("6379f0bd18d4b20100c041db", "s/v"),
    ("637e920b24b7fd0100a15e6a", "?"),
    ("6380907c8142b10100e653cf ", "szc"),
    ("63850dde83aa9b01007df5b6", "z"),
};

var except = personIdQBluprintList.Select(x => x.Item1).ToList();

//var googleDocId = "1BcD5YVSTon0_vngf-RNpDCBtj0U1TuoAcC8ZktTajbw";
var googleDocId = "1cmf1XM4nvNhgzNRjJES9lBAGp2ZgbC7fKR0RUco8QQY";


tinderOperations.ExportAllAccountMatchesToGoogleDoc(googleDocId, kamilStareId, except);


Console.WriteLine("Finished");
Console.WriteLine("Finished");

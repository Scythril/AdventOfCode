using System;
using System.Collections.Generic;

namespace AdventOfCode2018.Day2
{
    class Day2
    {
        public int Part1(string[] input)
        {
            var twoCount = 0;
            var threeCount = 0;

            foreach (var line in input)
            {
                var charDictionary = new Dictionary<char, int>();
                foreach (var character in line.ToCharArray())
                {
                    if (charDictionary.ContainsKey(character))
                    {
                        charDictionary[character]++;
                    }
                    else
                    {
                        charDictionary.Add(character, 1);
                    }
                }

                if (charDictionary.ContainsValue(2))
                {
                    twoCount++;
                }

                if (charDictionary.ContainsValue(3))
                {
                    threeCount++;
                }
            }

            return twoCount * threeCount;
        }

        public string Part2(string[] input)
        {
            for (int mainPos = 0; mainPos < input.Length; mainPos++)
            {
                var mainChars = input[mainPos].ToCharArray();
                for (int currPos = mainPos + 1; currPos < input.Length; currPos++)
                {
                    var currChars = input[currPos].ToCharArray();
                    var matches = "";
                    for (int arrayPointer = 0; arrayPointer < mainChars.Length; arrayPointer++)
                    {
                        if (mainChars[arrayPointer] == currChars[arrayPointer])
                        {
                            matches += mainChars[arrayPointer];
                        }
                    }

                    if (matches.Length == currChars.Length - 1)
                    {
                        return matches;
                    }
                }
            }

            throw new ArgumentException("No 2 IDs differ by 1 character.");
        }

        public static void Run()
        {
            var input = GetInput().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var Day2 = new Day2();
            Console.WriteLine("\n###############\n###############\nDay2\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(Day2.Part1(new[] { "abcdef", "bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab" }));
            Console.WriteLine(Day2.Part1(input));

            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(Day2.Part2(@"abcde
fghij
klmno
pqrst
fguij
axcye
wvxyz".Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)));
            Console.WriteLine(Day2.Part2(input));
        }

        private static string GetInput()
        {
            return @"umdryebvlapkozostecnihjexg
amdryebalapkozfstwcnrhjqxg
umdcyebvlapaozfstwcnihjqgg
ymdryrbvlapkozfstwcuihjqxg
umdrsebvlapkozxstwcnihjqig
umdryibvlapkohfstwcnfhjqxg
umdryebvqapkozfatwcnihjqxs
umzrpebvlapkozfshwcnihjqxg
fmhryebvlapkozfstwckihjqxg
umdryebvlahkozfstwcnizjrxg
qmdryebvlapkozfslwcnihgqxg
umdiyebjlapknzfstwcnihjqxg
umdryebvlapkoqfstwcaihvqxg
cmdryebvlapkpzfstwcnihjvxg
umdryebvlakkozfstwcgihjixg
umdryebvlasjozfstwcnihqqxg
umdryebvladkozfsvwcnifjqxg
umdrlebvlapaozfstwcniwjqxg
umdryebvlhpkozrstwsnihjqxg
umdryebvcapkozfqtwcnihjrxg
ubdrykbvlapkowfstwcnihjqxg
umdryebvldpkozfstwcnihtqsg
umdryebvlapaozyutwcnihjqxg
umdryibvlapkozfstdfnihjqxg
umdryebvlapgozkstwznihjqxg
umdrxebvlapkozfstwcngxjqxg
umdryekvlapkozfstwclchjqxg
nmdryebvlapkozjsewcnihjqxg
umdryebvyapkozfstfcniheqxg
umdfyebvlapkozfstwcnhhjpxg
umdryelylupkozfstwcnihjqxg
smdryebvlqpkozfstwcnihjdxg
umdryebvlapaozfsuwcnihjqxc
umdryebvlrzkozrstwcnihjqxg
umdbycbvlapkojfstwcnihjqxg
umdryebvlapkonfstwpnirjqxg
uecryebvlapkozfstwcnihpqxg
uqdryebvltpkozfstwcnihrqxg
umdryebvlqsknzfstwcnihjqxg
cmdryebvlapkocfstwcvihjqxg
umdrkebvlapkozqsfwcnihjqxg
umdryabveapkoifstwcnihjqxg
ummrnehvlapkozfstwcnihjqxg
umdryebvlxpkozfstwqnihjtxg
umdryebvlagkozastwcnihjqxh
umdryebvlatkozzhtwcnihjqxg
umdryebvlcpkozfstwrnihjqvg
umdryebvlapkozfsnwcnrhjcxg
umdzyebvlypkozfstwcnibjqxg
nmdryebvlvpkozbstwcnihjqxg
uwdryebvlipkozfstwcnihvqxg
umdraebvlavkozfstwcnihjqwg
umdeyebvlspbozfstwcnihjqxg
umdryxlvlapkozfstwcnihjqxu
umdryegvlapkqzfstwcnirjqxg
umdrupbvlapkozfstwcnihjqog
imxryebvlapkxzfstwcnihjqxg
umdrfebvlapkozowtwcnihjqxg
umdreebvlapkozmstwczihjqxg
undryebdlapkozbstwcnihjqxg
umdryebvlapkpzfetwcnihjqxb
ymdnyebvlapkozfstwinihjqxg
umdryebvaapkozfstwcnihyqqg
umdryebvlapkzzwsrwcnihjqxg
umdrkebvlapkmzfskwcnihjqxg
umdrmebvlapkozfsvwcnidjqxg
umdlyehvlapkozfstwcnihjqkg
umnryebvlrpkozfstwjnihjqxg
uqdryebvlapxozfsawcnihjqxg
vmdruebvlapkozfstwcnihjqqg
umdryabviapkozistwcnihjqxg
umdryebvlapkzzfstwfnihkqxg
uvdryebvlapkozfsxwcuihjqxg
umdlhebvlapkozfstwcnvhjqxg
umdreebvlapkopfstjcnihjqxg
umdryebvlazkomfstwynihjqxg
kmdryebulapkoznstwcnihjqxg
umdryebvxakkozfstwinihjqxg
ukdryobvlapkozistwcnihjqxg
umdryebveapkozfstwcnthjqgg
mmdrtebvlapcozfstwcnihjqxg
umdryebvlapkolistwnnihjqxg
umdryebxlapkozfatwcnihjqxx
uxdryebvlapkozfstwhniheqxg
ufdryebvzapkozfstwcnbhjqxg
amdryhbvlapkozfstwcnifjqxg
umqryebvlaphozfstwcnihjqxn
umdryebvlapkosfstfcnihjqxe
gmkryebvlapkozfstwcnihjmxg
umdrnebvlkpkozfstwcnihjnxg
umdryebvrapkozfstmcndhjqxg
umdryebvmapkozfstichihjqxg
umdryesvnapkozestwcnihjqxg
umeryhbvlapkozfstfcnihjqxg
umdryedvbapkozfstwcnihqqxg
umdryebllapzozfstwcnihjvxg
umdcyebvlzdkozfstwcnihjqxg
umdrybbvlapkbvfstwcnihjqxg
umdrytbglapkozfsthcnihjqxg
umdryebvlkpkozfsteclihjqxg
umdntebvlapkmzfstwcnihjqxg
lkdryebveapkozfstwcnihjqxg
ymdryubvlapkozfstwbnihjqxg
tmrryebvlapkozfstwcnqhjqxg
umdryeovlaekonfstwcnihjqxg
umiryeuvlapkozfstwcnihjwxg
umdryebvlspvozwstwcnihjqxg
umdrtebvlapkoznxtwcnihjqxg
umvryebvlaphozfstwcnahjqxg
umdryebvlapkozfstinniajqxg
umdryebqlapkozfctwcnihjqxx
umdryebvlapkbzfptwcnihjqvg
umdryabviapkozistwcnihjqxd
umdryrbvlapkezfstscnihjqxg
umhryebvlapkozfstacnihxqxg
umdxyelvlapkozfitwcnihjqxg
umdryevvuapkozfstwcnihtqxg
uydrypbvxapkozfstwcnihjqxg
umdryebvlapkopfstwcnihzqxo
uedryebvlapkozistwceihjqxg
umdiyebvlapkozfgtwcnihjqxv
ymdryebvlapkozfsticniqjqxg
umbrkebvlapkozfslwcnihjqxg
umdryebliapkozbstwcnihjqxg
umvryebolapkozfstwcnihjqig
umdryeavbackozfstwcnihjqxg
umdryfbvlapsozfstwcnihaqxg
umdqyebvlapkozfjtgcnihjqxg
umdrjebvlaqkozfstwcyihjqxg
umdryebklaqkozrstwcnihjqxg
umdryebvpapkozfstwcpihjqjg
uydryebhlawkozfstwcnihjqxg
umdyyebvlapkozfstwcykhjqxg
umdryebvlapkozfstwcnitjnxh
umdzyebvlapkozfstwcnehyqxg
mmcryebvlapkozfstwinihjqxg
umdryebvlapuozfstwmvihjqxg
umdryfbvlapkozqstwcnihjmxg
umdryebslapsozfhtwcnihjqxg
umdtyemvlapmozfstwcnihjqxg
umdrxevvlapkozfytwcnihjqxg
umdahebvlapjozfstwcnihjqxg
umdryebvlapkozfstacnivjqxb
umdryebvlzpkozfjtwcnihjyxg
umdryebvlaqkozfstwcnisjqxu
umdrydbvlapkozfsuwcnihjlxg
umdryebvlapkomrstwcnihjqkg
umdryebvlapcozfstmcnwhjqxg
umdryebvlahkozfstwcibhjqxg
gmdrzebvlapkozlstwcnihjqxg
umdryebvlapkezfsswcnrhjqxg
umdryebvlapkoqfitwcgihjqxg
umdrnebvlapkozfsiwcninjqxg
umdryebvlapkozfsrwckohjqxg
umdryebtlapkomfstwcnihjexg
umdryxbvlapjozfstwcnihoqxg
umdpyebvlapkosustwcnihjqxg
umdryebvlapkvzfawwcnihjqxg
umhnyebvlaikozfstwcnihjqxg
umdryebvlagkozfstvknihjqxg
uodryebjlapkoxfstwcnihjqxg
umdryefdlapkozfstwcnyhjqxg
umprmebvtapkozfstwcnihjqxg
umdhyebvlapoozfstwcnihjqgg
uddryebvidpkozfstwcnihjqxg
umdryebtlapkozfetwfnihjqxg
umdbyebolapkozfstwcoihjqxg
umdryebvlapkonfstwcnihjpxo
umdryebvlapkohfstwcnihjqwk
umdryebolalkkzfstwcnihjqxg
updryebvxapkozfstwcnshjqxg
umdryebvlapkovfktwcnuhjqxg
umdrqrbvlppkozfstwcnihjqxg
umdrylgvlapkozfstwrnihjqxg
umdryebvlapkozfstxcnihbqig
uvdryeevlappozfstwcnihjqxg
zmdryebvlapkozfstwcnihqqxt
umdryebvlapvozfstwenihiqxg
umdryebvlbpkozfsgwcnihjlxg
umdryhbvlapkozfstwcnihtqxw
umdreecvlapkozwstwcnihjqxg
umwryebvlapkoztsmwcnihjqxg
ukdryebvfapkozrstwcnihjqxg
umdrylbdlamkozfstwcnihjqxg
umdryebvlapoozwsmwcnihjqxg
umdryebvlapkozfqtwcnnzjqxg
umdryekvlapktzfstwcnohjqxg
umdryebvlapkozfstwcnihjwqo
umdrrebflapkogfstwcnihjqxg
umdryevvlapkozfztwctihjqxg
umdrybbvlapkozfstwcnihxaxg
umdryebvlapkozfsowcnphjqag
smdryebvlapbozfitwcnihjqxg
umdryebvtapiozfstwcnihjqxe
umdryebjlakkozfstwccihjqxg
umdryebvlapdozfshwckihjqxg
umnryebvlapiozfstwcnihlqxg
umdrycbvlapkjzfsnwcnihjqxg
umdryebvyaprozjstwcnihjqxg
ucdryebvlapkozfstwomihjqxg
umdryebvlagklzfstwcnihjqyg
umdryebvladkozfstwcnihjqjh
umdrwebvlapkozfstwdnicjqxg
umdryebvlapkmzfstwcniheqxr
umdryebvlapkjzfstwcviheqxg
umdrvebvlapkozfstwcbihjqmg
umdrfebvlapkoffstwcnihsqxg
umdryebvtarkazfstwcnihjqxg
umdryebvlapkozfstwcfihjcng
umdryebvlapkktostwcnihjqxg
uedryeevlapkozfstwcniijqxg
bmdryebylapkozfstwcnihjqog
umdryebvlmpkoztstwcnihjqeg
umdryepvlarkohfstwcnihjqxg
uwdryebvlapklzfstzcnihjqxg
umdryebklapkozfsswcbihjqxg
umdtyeavlapkozfstwsnihjqxg
umdryebvaapkozfhtfcnihjqxg
umdrpebvlapuozfstwvnihjqxg
umdryebvlapkozffmwcniijqxg
uqdpyebvlapkozfstwfnihjqxg
umdryebvlapuozdstwcnihjhxg
tmdryhbvlapkozfptwcnihjqxg
umdryevvmapkozfstwcnihjgxg
umdryeuvlapmozfstwcnihjwxg
umdryebqlzpkozfbtwcnihjqxg
umdryebvsapkozystwcniqjqxg
imdryebvlapkozfscwinihjqxg
umdryebvlzpkozustwcnmhjqxg
umdrypbvlapbozfsnwcnihjqxg
bmdryebvlapqoznstwcnihjqxg
umdrfebvlapaozfstwcnihxqxg
umdiyebvxapkozfstwcnchjqxg
umdrygbvlapkozfstwcnizjqxz
amdryedvlapkozfstwcnihfqxg
umdryebvvapzozfstwcnihjgxg
undryebvlapkzzfstjcnihjqxg
umdryvbvlapgozfrtwcnihjqxg
umdrkebvlapkozfstwcnihihxg
umdryebvrppkozfsowcnihjqxg
umdryebvlapktzfsdwclihjqxg
otdrdebvlapkozfstwcnihjqxg
mmdryebvlazkozfxtwcnihjqxg
umdryebvlapkozfsbwtnihjqxa
imqryebvrapkozfstwcnihjqxg
umdryebvlrpkozfscwcnihjqlg
uedryebvlapkoznsvwcnihjqxg
umdryebvlqpkozfstscnihjqxj
umerycbvlapkozfstwcnihjqxh
umdkykbvlapjozfstwcnihjqxg";
        }
    }
}

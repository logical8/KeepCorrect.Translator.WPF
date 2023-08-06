using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KeepCorrect.Translator
{
    
    
    public partial class SearchResult
    {
        [JsonProperty("play_word")]
        public string PlayWord { get; set; }

        [JsonProperty("allAddedTranslations")]
        public List<object> AllAddedTranslations { get; set; }

        [JsonProperty("word_videos")]
        public List<object> WordVideos { get; set; }

        [JsonProperty("word_phrases")]
        public List<object> WordPhrases { get; set; }

        [JsonProperty("configAutoPlay")]
        public ConfigAutoPlay ConfigAutoPlay { get; set; }

        [JsonProperty("Word")]
        public Word Word { get; set; }

        [JsonProperty("word_speakers")]
        public List<string> WordSpeakers { get; set; }

        [JsonProperty("word_speakers_slow")]
        public List<string> WordSpeakersSlow { get; set; }

        [JsonProperty("hide_accents")]
        public HideAccents HideAccents { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }
    }

    public partial class ConfigAutoPlay
    {
        [JsonProperty("auto_play_word_show_balloon")]
        public long AutoPlayWordShowBalloon { get; set; }

        [JsonProperty("is_was_auto_play_word")]
        public bool IsWasAutoPlayWord { get; set; }

        [JsonProperty("auto_play_video_balloon")]
        public long AutoPlayVideoBalloon { get; set; }
    }

    public partial class HideAccents
    {
        [JsonProperty("uk")]
        public bool Uk { get; set; }

        [JsonProperty("us")]
        public bool Us { get; set; }

        [JsonProperty("au")]
        public bool Au { get; set; }
    }

    public partial class Word
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("base_word_id")]
        public long BaseWordId { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("word")]
        public string WordWord { get; set; }

        [JsonProperty("base_word")]
        public string BaseWord { get; set; }

        [JsonProperty("transcriptions")]
        public Transcriptions Transcriptions { get; set; }

        [JsonProperty("transcription")]
        public Transcription Transcription { get; set; }

        [JsonProperty("translation")]
        public string Translation { get; set; }

        [JsonProperty("word_to_id")]
        public DefaultSynonymGroups WordToId { get; set; }

        [JsonProperty("default_translation")]
        public string DefaultTranslation { get; set; }

        [JsonProperty("default_synonym_group")]
        public long DefaultSynonymGroup { get; set; }

        [JsonProperty("default_synonym_groups")]
        public DefaultSynonymGroups DefaultSynonymGroups { get; set; }

        [JsonProperty("expression")]
        public string Expression { get; set; }

        [JsonProperty("exception")]
        public string Exception { get; set; }

        [JsonProperty("is_main")]
        public bool IsMain { get; set; }

        [JsonProperty("is_base_form")]
        public bool IsBaseForm { get; set; }

        [JsonProperty("is_exception")]
        public bool IsException { get; set; }

        [JsonProperty("is_expression")]
        public bool IsExpression { get; set; }

        [JsonProperty("noun_uncountable")]
        public bool NounUncountable { get; set; }

        [JsonProperty("lemma")]
        public string Lemma { get; set; }

        [JsonProperty("part_of_speech")]
        public string PartOfSpeech { get; set; }

        [JsonProperty("article")]
        public string Article { get; set; }

        [JsonProperty("parts_of_speech")]
        public PartsOfSpeech PartsOfSpeech { get; set; }

        [JsonProperty("base_forms")]
        public List<object> BaseForms { get; set; }

        [JsonProperty("parent_expression")]
        public List<object> ParentExpression { get; set; }

        [JsonProperty("exceptions")]
        public List<object> Exceptions { get; set; }

        [JsonProperty("post_id")]
        public long PostId { get; set; }

        [JsonProperty("piece_index")]
        public long PieceIndex { get; set; }

        [JsonProperty("vote_exists")]
        public bool VoteExists { get; set; }

        [JsonProperty("parent_word")]
        public string ParentWord { get; set; }

        [JsonProperty("user_video_id")]
        public object UserVideoId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("default_translations")]
        public DefaultTranslations DefaultTranslations { get; set; }

        [JsonProperty("exists")]
        public bool Exists { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("ws")]
        public string Ws { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("word")]
        public string Word { get; set; }

        [JsonProperty("lemma")]
        public string Lemma { get; set; }

        [JsonProperty("translate")]
        public string Translate { get; set; }

        [JsonProperty("part_of_speech")]
        public string PartOfSpeech { get; set; }

        [JsonProperty("list_parts_of_speech")]
        public string ListPartsOfSpeech { get; set; }

        [JsonProperty("transcription")]
        public string Transcription { get; set; }

        [JsonProperty("c1")]
        public string C1 { get; set; }

        [JsonProperty("pc")]
        public string Pc { get; set; }

        [JsonProperty("spelling")]
        public string Spelling { get; set; }

        [JsonProperty("coca")]
        public long Coca { get; set; }

        [JsonProperty("pcoca")]
        public string Pcoca { get; set; }

        [JsonProperty("cc1")]
        public string Cc1 { get; set; }

        [JsonProperty("ccoca")]
        public string Ccoca { get; set; }

        [JsonProperty("cpcoca")]
        public string Cpcoca { get; set; }

        [JsonProperty("publish")]
        public long Publish { get; set; }

        [JsonProperty("admin_id")]
        public long AdminId { get; set; }

        [JsonProperty("frequency")]
        public long Frequency { get; set; }

        [JsonProperty("noun_uncountable")]
        public long NounUncountable { get; set; }

        [JsonProperty("verb_modal")]
        public long VerbModal { get; set; }

        [JsonProperty("verb_normal")]
        public long VerbNormal { get; set; }

        [JsonProperty("noun_exception")]
        public long NounException { get; set; }

        [JsonProperty("verb_exception")]
        public long VerbException { get; set; }

        [JsonProperty("adjective_exception")]
        public long AdjectiveException { get; set; }

        [JsonProperty("approved")]
        public long Approved { get; set; }

        [JsonProperty("expr_skip")]
        public long ExprSkip { get; set; }

        [JsonProperty("rand")]
        public string Rand { get; set; }

        [JsonProperty("transcription_uk")]
        public string TranscriptionUk { get; set; }

        [JsonProperty("transcription_us")]
        public string TranscriptionUs { get; set; }

        [JsonProperty("lemma_frequency_rank")]
        public long LemmaFrequencyRank { get; set; }

        [JsonProperty("lemma_frequency_group")]
        public long LemmaFrequencyGroup { get; set; }
    }

    public partial class DefaultSynonymGroups
    {
        [JsonProperty("part")]
        public long Part { get; set; }
    }

    public partial class DefaultTranslations
    {
        [JsonProperty("part")]
        public string Part { get; set; }
    }

    public partial class PartsOfSpeech
    {
        [JsonProperty("noun")]
        public Adjective Noun { get; set; }

        [JsonProperty("adverb")]
        public Adjective Adverb { get; set; }

        [JsonProperty("adjective")]
        public Adjective Adjective { get; set; }

        [JsonProperty("verb")]
        public Adjective Verb { get; set; }
    }

    public partial class Adjective
    {
        [JsonProperty("c1")]
        public string C1 { get; set; }

        [JsonProperty("name_en")]
        public string NameEn { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("part_of_speech")]
        public string PartOfSpeech { get; set; }

        [JsonProperty("part_of_speech_ru")]
        public string PartOfSpeechRu { get; set; }

        [JsonProperty("values")]
        public List<Value> Values { get; set; }

        [JsonProperty("article")]
        public string Article { get; set; }

        [JsonProperty("weight_total")]
        public double WeightTotal { get; set; }

        [JsonProperty("word")]
        public string Word { get; set; }

        [JsonProperty("word_id")]
        public long WordId { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("value")]
        public string ValueValue { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }

        [JsonProperty("precise_weight")]
        public string PreciseWeight { get; set; }

        [JsonProperty("synonym_group")]
        public long SynonymGroup { get; set; }

        [JsonProperty("is_main")]
        public bool IsMain { get; set; }

        [JsonProperty("votes_count")]
        public long VotesCount { get; set; }

        [JsonProperty("is_admin_recommended")]
        public long IsAdminRecommended { get; set; }
    }

    public partial class Transcription
    {
        [JsonProperty("uk")]
        public string Uk { get; set; }

        [JsonProperty("us")]
        public string Us { get; set; }
    }

    public partial class Transcriptions
    {
        [JsonProperty("part")]
        public Transcription Part { get; set; }
    }
}
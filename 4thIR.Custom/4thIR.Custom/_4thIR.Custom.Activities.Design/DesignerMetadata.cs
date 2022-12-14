using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using _4thIR.Custom.Activities.Design.Designers;
using _4thIR.Custom.Activities.Design.Properties;

namespace _4thIR.Custom.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            #region Process Scope
            builder.AddCustomAttributes(typeof(ProcessScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(ProcessScope), new DesignerAttribute(typeof(ProcessScopeDesigner)));
            builder.AddCustomAttributes(typeof(ProcessScope), new HelpKeywordAttribute(""));
            #endregion

            #region Document Classification KYC
            builder.AddCustomAttributes(typeof(DocumentClassificationKYC), new CategoryAttribute("4th-IR.Document Classification.KYC"));
            builder.AddCustomAttributes(typeof(DocumentClassificationKYC), new DesignerAttribute(typeof(DocumentClassificationKYCDesigner)));
            builder.AddCustomAttributes(typeof(DocumentClassificationKYC), new HelpKeywordAttribute(""));
            #endregion

            #region Text Translation - Transformers (Multilanguage)
            builder.AddCustomAttributes(typeof(TextTranslationTM), new CategoryAttribute("4th-IR.TextTranslation.Multilanguage"));
            builder.AddCustomAttributes(typeof(TextTranslationTM), new DesignerAttribute(typeof(TextTranslationTMDesigner)));
            builder.AddCustomAttributes(typeof(TextTranslationTM), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Vissl (RegnetY-60)
            builder.AddCustomAttributes(typeof(ImageClassificationVissl), new CategoryAttribute("4th-IR.Image Classification.Vissl"));
            builder.AddCustomAttributes(typeof(ImageClassificationVissl), new DesignerAttribute(typeof(ImageClassificationVisslDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassificationVissl), new HelpKeywordAttribute(""));
            #endregion

            #region ObjectDetection.EfficientDet
            builder.AddCustomAttributes(typeof(ObjectDetectionED), new CategoryAttribute("4th-IR.Object Detection.EfficientDet"));
            builder.AddCustomAttributes(typeof(ObjectDetectionED), new DesignerAttribute(typeof(ObjectDetectionEDDesigner)));
            builder.AddCustomAttributes(typeof(ObjectDetectionED), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Classy Vision
            builder.AddCustomAttributes(typeof(ImageClassificationCV), new CategoryAttribute("4th-IR.Image Classification.ClassyVision"));
            builder.AddCustomAttributes(typeof(ImageClassificationCV), new DesignerAttribute(typeof(ImageClassificationCVDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassificationCV), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Resnet 50
            builder.AddCustomAttributes(typeof(ImageClassificationR50), new CategoryAttribute("4th-IR.Image Classification.Resnet 50"));
            builder.AddCustomAttributes(typeof(ImageClassificationR50), new DesignerAttribute(typeof(ImageClassificationR50Designer)));
            builder.AddCustomAttributes(typeof(ImageClassificationR50), new HelpKeywordAttribute(""));
            #endregion

            #region Roberta Base Squad 2 - Question Answer
            builder.AddCustomAttributes(typeof(QuestionAnsweringRBS2), new CategoryAttribute("4th-IR.Question Answering.Roberta Base Squad 2"));
            builder.AddCustomAttributes(typeof(QuestionAnsweringRBS2), new DesignerAttribute(typeof(QuestionAnsweringRBS2Designer)));
            builder.AddCustomAttributes(typeof(QuestionAnsweringRBS2), new HelpKeywordAttribute(""));
            #endregion

            #region Text Summarization - BigBird
            builder.AddCustomAttributes(typeof(TextSummarizationBB), new CategoryAttribute("4th-IR.Text Summarization.Big Bird"));
            builder.AddCustomAttributes(typeof(TextSummarizationBB), new DesignerAttribute(typeof(TextSummarizationBBDesigner)));
            builder.AddCustomAttributes(typeof(TextSummarizationBB), new HelpKeywordAttribute(""));
            #endregion

            #region Part of Speech - Flair
            builder.AddCustomAttributes(typeof(PartOfSpeechIdentificationFlair), new CategoryAttribute("4th-IR.Part of Speech.Flair"));
            builder.AddCustomAttributes(typeof(PartOfSpeechIdentificationFlair), new DesignerAttribute(typeof(PartOfSpeechIdentificationFlairDesigner)));
            builder.AddCustomAttributes(typeof(PartOfSpeechIdentificationFlair), new HelpKeywordAttribute(""));
            #endregion

            #region Name Entity Recognition - Flair
            builder.AddCustomAttributes(typeof(NameEntityRecognitionFlair), new CategoryAttribute("4th-IR.Name Entity Recognition.Flair"));
            builder.AddCustomAttributes(typeof(NameEntityRecognitionFlair), new DesignerAttribute(typeof(NameEntityRecognitionFlairDesigner)));
            builder.AddCustomAttributes(typeof(NameEntityRecognitionFlair), new HelpKeywordAttribute(""));
            #endregion

            #region Zero Shot Classification - Roberta
            builder.AddCustomAttributes(typeof(TextClassificationZSR), new CategoryAttribute("4th-IR.Text Classification.Zero Shot.Roberta"));
            builder.AddCustomAttributes(typeof(TextClassificationZSR), new DesignerAttribute(typeof(TextClassificationZSRDesigner)));
            builder.AddCustomAttributes(typeof(TextClassificationZSR), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Facebook Deit Base
            builder.AddCustomAttributes(typeof(ImageClassificationFDeitBase), new CategoryAttribute("4th-IR.Image Classification.Facebook.Deit Base"));
            builder.AddCustomAttributes(typeof(ImageClassificationFDeitBase), new DesignerAttribute(typeof(ImageClassificationFDeitBaseDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassificationFDeitBase), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Facebook Dit Base
            builder.AddCustomAttributes(typeof(ImageClassificationFDitBase), new CategoryAttribute("4th-IR.Image Classification.Facebook.Dit Base"));
            builder.AddCustomAttributes(typeof(ImageClassificationFDitBase), new DesignerAttribute(typeof(ImageClassificationFDitBaseDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassificationFDitBase), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Microsoft Swin Base
            builder.AddCustomAttributes(typeof(ImageClassificationMSwinBase), new CategoryAttribute("4th-IR.Image Classification.Microsoft.Swin Base"));
            builder.AddCustomAttributes(typeof(ImageClassificationMSwinBase), new DesignerAttribute(typeof(ImageClassificationMSwinBaseDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassificationMSwinBase), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Vit Base Bean
            builder.AddCustomAttributes(typeof(ImageClassificationVitBaseBean), new CategoryAttribute("4th-IR.Image Classification.Vit Base.Bean"));
            builder.AddCustomAttributes(typeof(ImageClassificationVitBaseBean), new DesignerAttribute(typeof(ImageClassificationVitBaseBeanDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassificationVitBaseBean), new HelpKeywordAttribute(""));
            #endregion

            #region Text Summarization - Bart Large
            builder.AddCustomAttributes(typeof(TextSummarizerBartLarge), new CategoryAttribute("4th-IR.Text Summarization.Bart.large"));
            builder.AddCustomAttributes(typeof(TextSummarizerBartLarge), new DesignerAttribute(typeof(TextSummarizerBartLargeDesigner)));
            builder.AddCustomAttributes(typeof(TextSummarizerBartLarge), new HelpKeywordAttribute(""));
            #endregion

            #region Text Summarization - Bert2Bert Small
            builder.AddCustomAttributes(typeof(TextSummarizationBert2BertSmall), new CategoryAttribute("4th-IR.Text Summarization.Bert2Bert.Small"));
            builder.AddCustomAttributes(typeof(TextSummarizationBert2BertSmall), new DesignerAttribute(typeof(TextSummarizationBert2BertSmallDesigner)));
            builder.AddCustomAttributes(typeof(TextSummarizationBert2BertSmall), new HelpKeywordAttribute(""));
            #endregion

            #region Text Summarization - Fairseq
            builder.AddCustomAttributes(typeof(TextSummarizationFairseq), new CategoryAttribute("4th-IR.Text Summarization.Fairseq"));
            builder.AddCustomAttributes(typeof(TextSummarizationFairseq), new DesignerAttribute(typeof(TextSummarizationFairseqDesigner)));
            builder.AddCustomAttributes(typeof(TextSummarizationFairseq), new HelpKeywordAttribute(""));
            #endregion

            #region Text Translation - Transformers(English, French, Romanian, German) Machine Translation 2
            builder.AddCustomAttributes(typeof(TextTranslationMtTransformers2), new CategoryAttribute("4th-IR.Text Translation.Machine Translation.Transformers 2"));
            builder.AddCustomAttributes(typeof(TextTranslationMtTransformers2), new DesignerAttribute(typeof(TextTranslationMtTransformers2Designer)));
            builder.AddCustomAttributes(typeof(TextTranslationMtTransformers2), new HelpKeywordAttribute(""));
            #endregion

            #region Text Generation - Fairseq
            builder.AddCustomAttributes(typeof(TextGenerationFairseq), new CategoryAttribute("4th-IR.Text Generation.Fairseq"));
            builder.AddCustomAttributes(typeof(TextGenerationFairseq), new DesignerAttribute(typeof(TextGenerationFairseqDesigner)));
            builder.AddCustomAttributes(typeof(TextGenerationFairseq), new HelpKeywordAttribute(""));
            #endregion

            #region Name Entity Recognition - Fairseq (Multitagging and Deeptagging)
            builder.AddCustomAttributes(typeof(NameEntityRecognitionFairseq), new CategoryAttribute("4th-IR.Name Entity Recognition.Fairseq"));
            builder.AddCustomAttributes(typeof(NameEntityRecognitionFairseq), new DesignerAttribute(typeof(NameEntityRecognitionFairseqDesigner)));
            builder.AddCustomAttributes(typeof(NameEntityRecognitionFairseq), new HelpKeywordAttribute(""));
            #endregion

            #region Name Entity Recognition - BERT
            builder.AddCustomAttributes(typeof(NameEntityRecognitionBERT), new CategoryAttribute("4th-IR.Name Entity Recognition.BERT"));
            builder.AddCustomAttributes(typeof(NameEntityRecognitionBERT), new DesignerAttribute(typeof(NameEntityRecognitionBERTDesigner)));
            builder.AddCustomAttributes(typeof(NameEntityRecognitionBERT), new HelpKeywordAttribute(""));
            #endregion

            #region Text Generation - Transformers
            builder.AddCustomAttributes(typeof(TextGenerationTransformers), new CategoryAttribute("4th-IR.Text Generation.Transformers"));
            builder.AddCustomAttributes(typeof(TextGenerationTransformers), new DesignerAttribute(typeof(TextGenerationTransformersDesigner)));
            builder.AddCustomAttributes(typeof(TextGenerationTransformers), new HelpKeywordAttribute(""));
            #endregion

            #region Sentiment Analysis - DeBerta
            builder.AddCustomAttributes(typeof(SentimentAnalysisDeBerta), new CategoryAttribute("4th-IR.Sentiment Analysis.DeBerta"));
            builder.AddCustomAttributes(typeof(SentimentAnalysisDeBerta), new DesignerAttribute(typeof(SentimentAnalysisDeBertaDesigner)));
            builder.AddCustomAttributes(typeof(SentimentAnalysisDeBerta), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - EfficientNetB1
            builder.AddCustomAttributes(typeof(ImageClassificationEfficientNetB1), new CategoryAttribute("4th-IR.Image Classification.EfficientNetB1"));
            builder.AddCustomAttributes(typeof(ImageClassificationEfficientNetB1), new DesignerAttribute(typeof(ImageClassificationEfficientNetB1Designer)));
            builder.AddCustomAttributes(typeof(ImageClassificationEfficientNetB1), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Facebook Levit 128S
            builder.AddCustomAttributes(typeof(ImageClassificationFacebookLevit128S), new CategoryAttribute("4th-IR.Image Classification.Facebook.Levit 128S"));
            builder.AddCustomAttributes(typeof(ImageClassificationFacebookLevit128S), new DesignerAttribute(typeof(ImageClassificationFacebookLevit128SDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassificationFacebookLevit128S), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification (MXNET-Resnet50)
            builder.AddCustomAttributes(typeof(ImageClassificationMXNETResnet50), new CategoryAttribute("4th-IR.Image Classification.MXNET.Resnet50"));
            builder.AddCustomAttributes(typeof(ImageClassificationMXNETResnet50), new DesignerAttribute(typeof(ImageClassificationMXNETResnet50Designer)));
            builder.AddCustomAttributes(typeof(ImageClassificationMXNETResnet50), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Google Vit Base
            builder.AddCustomAttributes(typeof(ImageClassificationGoogleVitBase), new CategoryAttribute("4th-IR.Image Classification.Google Vit Base"));
            builder.AddCustomAttributes(typeof(ImageClassificationGoogleVitBase), new DesignerAttribute(typeof(ImageClassificationGoogleVitBaseDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassificationGoogleVitBase), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - ViT-Base
            builder.AddCustomAttributes(typeof(ImageClassificationViTBase), new CategoryAttribute("4th-IR.Image Classification.Vit Base"));
            builder.AddCustomAttributes(typeof(ImageClassificationViTBase), new DesignerAttribute(typeof(ImageClassificationViTBaseDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassificationViTBase), new HelpKeywordAttribute(""));
            #endregion

            #region Image Classification - Google Vit Base (Fine-tuned)
            builder.AddCustomAttributes(typeof(ImageClassificationGoogleVitBaseFineTuned), new CategoryAttribute("4th-IR.Image Classification.Google Vit Base.Fine Tuned"));
            builder.AddCustomAttributes(typeof(ImageClassificationGoogleVitBaseFineTuned), new DesignerAttribute(typeof(ImageClassificationGoogleVitBaseFineTunedDesigner)));
            builder.AddCustomAttributes(typeof(ImageClassificationGoogleVitBaseFineTuned), new HelpKeywordAttribute(""));
            #endregion
           
            #region Object Detection-FasterRCNN Resnet50fpn
            builder.AddCustomAttributes(typeof(ObjectDetectionFasterRCNNResnet50fpn), new CategoryAttribute("4th-IR.Object Detection.Faster RCNN.Resnet 50fpn"));
            builder.AddCustomAttributes(typeof(ObjectDetectionFasterRCNNResnet50fpn), new DesignerAttribute(typeof(ObjectDetectionFasterRCNNResnet50fpnDesigner)));
            builder.AddCustomAttributes(typeof(ObjectDetectionFasterRCNNResnet50fpn), new HelpKeywordAttribute(""));
            #endregion

            #region Formula Recognition OpenVino
            builder.AddCustomAttributes(typeof(FormulaRecognitionOpenVino), new CategoryAttribute("4th-IR.Formula Recognition.OpenVino"));
            builder.AddCustomAttributes(typeof(FormulaRecognitionOpenVino), new DesignerAttribute(typeof(FormulaRecognitionOpenVinoDesigner)));
            builder.AddCustomAttributes(typeof(FormulaRecognitionOpenVino), new HelpKeywordAttribute(""));
            #endregion

            #region Handwritten Recognition for Engish, Japanese and Chinese Openvino
            builder.AddCustomAttributes(typeof(HandwrittenTextRecognitionOpenVino), new CategoryAttribute("4th-IR.Handwritten Recognition.Open Vino"));
            builder.AddCustomAttributes(typeof(HandwrittenTextRecognitionOpenVino), new DesignerAttribute(typeof(HandwrittenTextRecognitionOpenVinoDesigner)));
            builder.AddCustomAttributes(typeof(HandwrittenTextRecognitionOpenVino), new HelpKeywordAttribute(""));
            #endregion

            #region Object Detection Detectron2
            builder.AddCustomAttributes(typeof(ObjectDetectionDetectron2), new CategoryAttribute("4th-IR.Object Detection.Detectron 2"));
            builder.AddCustomAttributes(typeof(ObjectDetectionDetectron2), new DesignerAttribute(typeof(ObjectDetectionDetectron2Designer)));
            builder.AddCustomAttributes(typeof(ObjectDetectionDetectron2), new HelpKeywordAttribute(""));
            #endregion

            #region Object Detection - Yolov4
            builder.AddCustomAttributes(typeof(ObjectDetectionYolov4), new CategoryAttribute("4th-IR.Object Detection.Yolov 4"));
            builder.AddCustomAttributes(typeof(ObjectDetectionYolov4), new DesignerAttribute(typeof(ObjectDetectionYolov4Designer)));
            builder.AddCustomAttributes(typeof(ObjectDetectionYolov4), new HelpKeywordAttribute(""));
            #endregion

            #region Pose Estimation mmPose
            builder.AddCustomAttributes(typeof(PoseEstimationMmPose), new CategoryAttribute("4th-IR.Pose Estimation.mmPose"));
            builder.AddCustomAttributes(typeof(PoseEstimationMmPose), new DesignerAttribute(typeof(PoseEstimationMmPoseDesigner)));
            builder.AddCustomAttributes(typeof(PoseEstimationMmPose), new HelpKeywordAttribute(""));
            #endregion

            #region Pose Estimation Resnet 101
            builder.AddCustomAttributes(typeof(PoseEstimationResnet101), new CategoryAttribute("4th-IR.Pose Estimation.Resnet 101"));
            builder.AddCustomAttributes(typeof(PoseEstimationResnet101), new DesignerAttribute(typeof(PoseEstimationResnet101Designer)));
            builder.AddCustomAttributes(typeof(PoseEstimationResnet101), new HelpKeywordAttribute(""));
            #endregion

            #region  Semantic Segmentation- PSPNet
            builder.AddCustomAttributes(typeof(SemanticSegmentationPSPNet), new CategoryAttribute("4th-IR.Semantic Segmentation.PSPNet"));
            builder.AddCustomAttributes(typeof(SemanticSegmentationPSPNet), new DesignerAttribute(typeof(SemanticSegmentationPSPNetDesigner)));
            builder.AddCustomAttributes(typeof(SemanticSegmentationPSPNet), new HelpKeywordAttribute(""));
            #endregion

            # region Semantic Segmentation- Segformer
            builder.AddCustomAttributes(typeof(SemanticSegmantationSegformer), new CategoryAttribute("4th-IR.Semantic Segmentation.Segformer"));
            builder.AddCustomAttributes(typeof(SemanticSegmantationSegformer), new DesignerAttribute(typeof(SemanticSegmantationSegformerDesigner)));
            builder.AddCustomAttributes(typeof(SemanticSegmantationSegformer), new HelpKeywordAttribute(""));
            #endregion

            #region Age Classification- Google Vit Base
            builder.AddCustomAttributes(typeof(AgeClassificationGoogleVitBase), new CategoryAttribute("4th-IR.Age Classification.Google Vit Base"));
            builder.AddCustomAttributes(typeof(AgeClassificationGoogleVitBase), new DesignerAttribute(typeof(AgeClassificationGoogleVitBaseDesigner)));
            builder.AddCustomAttributes(typeof(AgeClassificationGoogleVitBase), new HelpKeywordAttribute(""));
            #endregion

            #region Document Classification- VLA
            builder.AddCustomAttributes(typeof(DocumentAnalysisVLA), new CategoryAttribute("4th-IR.Document Classification.VLA"));
            builder.AddCustomAttributes(typeof(DocumentAnalysisVLA), new DesignerAttribute(typeof(DocumentAnalysisVLADesigner)));
            builder.AddCustomAttributes(typeof(DocumentAnalysisVLA), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DocumentClassificationVLA), new CategoryAttribute("4th-IR.Document Classification.VLA"));
            builder.AddCustomAttributes(typeof(DocumentClassificationVLA), new DesignerAttribute(typeof(DocumentClassificationVLADesigner)));
            builder.AddCustomAttributes(typeof(DocumentClassificationVLA), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DocumentTextExtractionVLA), new CategoryAttribute("4th-IR.Document Classification.VLA"));
            builder.AddCustomAttributes(typeof(DocumentTextExtractionVLA), new DesignerAttribute(typeof(DocumentTextExtractionVLADesigner)));
            builder.AddCustomAttributes(typeof(DocumentTextExtractionVLA), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DocumentMetaExtractionVLA), new CategoryAttribute("4th-IR.Document Classification.VLA"));
            builder.AddCustomAttributes(typeof(DocumentMetaExtractionVLA), new DesignerAttribute(typeof(DocumentMetaExtractionVLADesigner)));
            builder.AddCustomAttributes(typeof(DocumentMetaExtractionVLA), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DocumentTextAnalysisVLA), new CategoryAttribute("4th-IR.Document Classification.VLA"));
            builder.AddCustomAttributes(typeof(DocumentTextAnalysisVLA), new DesignerAttribute(typeof(DocumentTextAnalysisVLADesigner)));
            builder.AddCustomAttributes(typeof(DocumentTextAnalysisVLA), new HelpKeywordAttribute(""));
            #endregion

            #region QA bert-large-uncased-whole-word-masking-squad
            builder.AddCustomAttributes(typeof(DocumentQuestionAnsweringBertLarge), new CategoryAttribute("4th-IR.Question Answering.Bert.Large"));
            builder.AddCustomAttributes(typeof(DocumentQuestionAnsweringBertLarge), new DesignerAttribute(typeof(DocumentQuestionAnsweringBertLargeDesigner)));
            builder.AddCustomAttributes(typeof(DocumentQuestionAnsweringBertLarge), new HelpKeywordAttribute(""));
            #endregion

            #region MultiTurn Conversation, Text Generation - DialoGPT
            builder.AddCustomAttributes(typeof(DialogueGenerationMultiTurnDialoGPT), new CategoryAttribute("4th-IR.Text Generation.Multi-Turn Conversation"));
            builder.AddCustomAttributes(typeof(DialogueGenerationMultiTurnDialoGPT), new DesignerAttribute(typeof(DialogueGenerationMultiTurnDialoGPTDesigner)));
            builder.AddCustomAttributes(typeof(DialogueGenerationMultiTurnDialoGPT), new HelpKeywordAttribute(""));
            #endregion

            #region Text Generation- Gpt2
            builder.AddCustomAttributes(typeof(TextGenerationGpt2), new CategoryAttribute("4th-IR.Text Generation.Gpt2"));
            builder.AddCustomAttributes(typeof(TextGenerationGpt2), new DesignerAttribute(typeof(TextGenerationGpt2Designer)));
            builder.AddCustomAttributes(typeof(TextGenerationGpt2), new HelpKeywordAttribute(""));
            #endregion

            #region Text Generation- OpenaiGpt
            builder.AddCustomAttributes(typeof(TextGenerationOpenaiGpt), new CategoryAttribute("4th-IR.Text Generation.Open AI Gpt"));
            builder.AddCustomAttributes(typeof(TextGenerationOpenaiGpt), new DesignerAttribute(typeof(TextGenerationOpenaiGptDesigner)));
            builder.AddCustomAttributes(typeof(TextGenerationOpenaiGpt), new HelpKeywordAttribute(""));
            #endregion

            #region POS Tagging - Transformer XL
            builder.AddCustomAttributes(typeof(TextCompletionTransformerXL), new CategoryAttribute("4th-IR.Text Completion.Transformer XL"));
            builder.AddCustomAttributes(typeof(TextCompletionTransformerXL), new DesignerAttribute(typeof(TextCompletionTransformerXLDesigner)));
            builder.AddCustomAttributes(typeof(TextCompletionTransformerXL), new HelpKeywordAttribute(""));
            #endregion

            #region Question and Answering Intel DynamicTinybert
            builder.AddCustomAttributes(typeof(QuestionAnsweringIntelDynamicTinybert), new CategoryAttribute("4th-IR.Question Answering.Intel.Dynamic Tinybert"));
            builder.AddCustomAttributes(typeof(QuestionAnsweringIntelDynamicTinybert), new DesignerAttribute(typeof(QuestionAnsweringIntelDynamicTinybertDesigner)));
            builder.AddCustomAttributes(typeof(QuestionAnsweringIntelDynamicTinybert), new HelpKeywordAttribute(""));
            #endregion

            #region XLM Roberta Base Squad 2 - Question Answer (German)
            builder.AddCustomAttributes(typeof(QuestionAnsweringXLMRobertaBaseSquad2), new CategoryAttribute("4th-IR.Question Answering.XLM.Roberta Base Squad 2"));
            builder.AddCustomAttributes(typeof(QuestionAnsweringXLMRobertaBaseSquad2), new DesignerAttribute(typeof(QuestionAnsweringXLMRobertaBaseSquad2Designer)));
            builder.AddCustomAttributes(typeof(QuestionAnsweringXLMRobertaBaseSquad2), new HelpKeywordAttribute(""));
            #endregion

            #region Text Summarization - Meeting Summary (Facebook Bart Large)
            builder.AddCustomAttributes(typeof(TextSummarizationMeetingSummaryFacebookBartLarge), new CategoryAttribute("4th-IR.Text Summarization.Meeting Summary.Facebook Bart Large"));
            builder.AddCustomAttributes(typeof(TextSummarizationMeetingSummaryFacebookBartLarge), new DesignerAttribute(typeof(TextSummarizationMeetingSummaryFacebookBartLargeDesigner)));
            builder.AddCustomAttributes(typeof(TextSummarizationMeetingSummaryFacebookBartLarge), new HelpKeywordAttribute(""));
            #endregion

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}

<script setup lang="ts">
import type { IMessageAttachment } from '@/_Compostables/GraphQL/Messages/MessageAttachment/IMessageAttachment';
import { useModelValue } from '@/_Compostables/Utility/useModelValue';
import { useAttachmentIsImage } from '@/_Compostables/GraphQL/Messages/MessageAttachment/useAttachmentIsImage';
import { useAttachmentIsAudio } from '@/_Compostables/GraphQL/Messages/MessageAttachment/useAttachmentIsAudio';
import { useAttachmentIsVideo } from '@/_Compostables/GraphQL/Messages/MessageAttachment/useAttachmentIsVideo';
import MessageAttachmentImage from '@/Components/Rows/MessageAttachmentImage.vue';
import MessageAttachmentAudio from '@/Components/Rows/MessageAttachmentAudio.vue';
import MessageAttachmentVideo from '@/Components/Rows/MessageAttachmentVideo.vue';

const props = defineProps<{
	modelValue?: IMessageAttachment,
	value?: IMessageAttachment,
}>();

const emit = defineEmits<{
	(e: 'update:modelValue', payload: IMessageAttachment | null): void,
	(e: 'on-value-changed', payload: IMessageAttachment | null): void,
}>();

const model = useModelValue(emit, props, null as IMessageAttachment | null);

const isImage = useAttachmentIsImage(model);
const isAudio = useAttachmentIsAudio(model);
const isVideo = useAttachmentIsVideo(model);

</script>
<template>
	<MessageAttachmentImage
		v-if="model && isImage"
		v-model="model"
		/>
	<MessageAttachmentAudio
		v-if="model && isAudio"
		v-model="model"
		/>
	<MessageAttachmentVideo
		v-if="model && isVideo"
		v-model="model"
		/>
</template>
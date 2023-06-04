<script setup lang="ts">
import type { IMessageAttachment } from '@/_Compostables/GraphQL/Messages/MessageAttachment/IMessageAttachment';
import { useModelValue } from '@/_Compostables/Utility/useModelValue';
import { useURI } from '@/_Compostables/GraphQL/Messages/MessageAttachment/useURI';
import { useMediaType } from '@/_Compostables/GraphQL/Messages/MessageAttachment/useMediaType';
import IconDownload from '@/Components/SVG/Icons/IconDownload.vue';

const props = defineProps<{
	modelValue?: IMessageAttachment,
	value?: IMessageAttachment,
}>();

const emit = defineEmits<{
	(e: 'update:modelValue', payload: IMessageAttachment | null): void,
	(e: 'on-value-changed', payload: IMessageAttachment | null): void,
}>();

const model = useModelValue(emit, props, null as IMessageAttachment | null);

const uri = useURI(model);
const mediaType = useMediaType(model);

</script>
<template>
	<div class="flex flex-col">
		<audio controls>
			<source :src="uri" :type="mediaType || undefined" />
		</audio>
		<a
			download="download"
			target="_blank"
			class="flex flex-row gap-1 items-center py-2"
			:href="uri"
			>
			<IconDownload class="w-6 h-6" />
			<div>Download File</div>
		</a>
	</div>
</template>
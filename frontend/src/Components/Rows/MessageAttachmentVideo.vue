<script setup lang="ts">
import type { IMessageAttachment } from '@/_Compostables/GraphQL/Messages/MessageAttachment/IMessageAttachment';
import { useURI } from '@/_Compostables/GraphQL/Messages/MessageAttachment/useURI';
import { useModelValue } from '@/_Compostables/Utility/useModelValue';
import IconDownload from '@/Components/SVG/Icons/IconDownload.vue';
import { useMediaType } from '@/_Compostables/GraphQL/Messages/MessageAttachment/useMediaType';

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
		<video controls>
			<source :src="uri" :type="mediaType || undefined" />
		</video>
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
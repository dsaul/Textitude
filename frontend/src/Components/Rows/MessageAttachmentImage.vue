<script setup lang="ts">
import { computed } from 'vue';
import type { IMessageAttachment } from '@/_Compostables/GraphQL/Messages/MessageAttachment/IMessageAttachment';
import { useModelValue } from '@/_Compostables/Utility/useModelValue';
import { useURI } from '@/_Compostables/GraphQL/Messages/MessageAttachment/useURI';
import IconDownload from '@/Components/SVG/Icons/IconDownload.vue';
import isEmpty from '@/Utility/isEmpty';

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

const pictureStyles = computed(() => {
	const ret: Record<string, string> = {};
	if (model.value && !isEmpty(`${model.value?.imageWidthPx || ''}`) && !isEmpty(`${model.value?.imageHeightPx || ''}`)) {
		ret['max-width'] = `${model.value.imageWidthPx}px`;
		ret['max-height'] = `${model.value.imageHeightPx}px`;
	}
	
	return ret;
});

</script>
<template>
	<div class="flex flex-col">
		<a
			target="_blank"
			:href="uri"
			>
			<picture
				:style="pictureStyles"
				>
				<img :src="uri" alt="">
			</picture>
		</a>
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
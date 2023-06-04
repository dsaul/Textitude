<script setup lang="ts">

import isEmpty from '@/Utility/isEmpty';
import { usePropertyWatcherUpdate } from '@/_Compostables/Utility/usePropertyWatcherUpdate';
import { computed, onMounted, watch, ref, nextTick } from 'vue';

// Props
const props = defineProps<{
	src: string,
	classNames?: string;
	invalidClassNames?: string;
	styleDeclarations?: '',
	rounded?: 'none' | 'full' | 'xl';
	delayMSUntilFallback?: number;
}>();

const src = ref<string>('');
const classNames = ref<string>('');
const invalidClassNames = ref<string>('');
const rounded = ref<'none' | 'full' | 'xl'>('none');
const delayMSUntilFallback = ref<number>(250);

// Locals
const imageTestEl = ref<HTMLImageElement | null>(null);
const imageIsValid = ref<boolean | null>(null);
const imageVisibleSourceURI = ref<string>('');

watch(props, (next, last) => {
	usePropertyWatcherUpdate(src, 'src', '', next, last, () => {
		if (isEmpty(src.value)) {
			imageVisibleSourceURI.value = '';
			imageIsValid.value = false;
			return;
		}
		
		nextTick(() => {
			beginLoadingImg(src.value);
		});
	});
	usePropertyWatcherUpdate(classNames, 'classNames', '', next, last);
	usePropertyWatcherUpdate(invalidClassNames, 'invalidClassNames', '', next, last);
	usePropertyWatcherUpdate(rounded, 'rounded', 'none', next, last);
	usePropertyWatcherUpdate(delayMSUntilFallback, 'delayMSUntilFallback', 250, next, last);
}, { immediate: true });

onMounted(() => {
	beginLoadingImg(src.value);
});

const beginLoadingImg = (src: string | null) => {
	
	// console.log('beginLoadingImg', src);
	
	
	
	if (isEmpty(imageVisibleSourceURI.value)) {
		// console.log('schedule timeout');
		setTimeout(() => {
			if (imageIsValid.value === null) {
				imageIsValid.value = false;
			}
			
			// console.log('go to fallback');
			
		}, delayMSUntilFallback.value);
	}
	
	if (src === null) {
		// console.warn('beginLoadingImg not loading as null');
		return;
	}
	
	imageTestEl.value = new Image();
	imageTestEl.value.src = isEmpty(src) ? '' : src;
	imageTestEl.value.onload = () => {
		imageIsValid.value = imageTestEl.value!.naturalHeight !== 0 && imageTestEl.value!.naturalWidth !== 0;
		// console.debug('image valid?',imageIsValid.value);
		
		if (imageIsValid.value) {
			imageVisibleSourceURI.value = src;
		}
	};
	imageTestEl.value.onerror = () => {
		imageIsValid.value = false;
	};
	
};

const classes = computed(() => {
	const ret: Record<string, true> = {};
	ret['inline-block shrink-0'] = true;
	switch (rounded.value) {
		default:
		case 'none': {
			break;
		}
		case 'full': {
			ret['rounded-full'] = true;
			break;
		}
		case 'xl': {
			ret['rounded-xl'] = true;
			break;
		}
	}
	if (classNames.value) {
		ret[classNames.value] = true;
	}
	if (invalidClassNames.value && !imageIsValid.value) {
		ret[invalidClassNames.value] = true;
	}
	
	return ret;
});

const classesImg = computed(() => {
	const ret: Record<string, true> = {};
	ret['object-cover w-full h-full'] = true;
	switch (rounded.value) {
		default:
		case 'none': {
			break;
		}
		case 'full': {
			ret['rounded-full'] = true;
			break;
		}
		case 'xl': {
			ret['rounded-xl'] = true;
			break;
		}
	}
	return ret;
});

</script>
<template>
	<div :class="classes" :style="`${styleDeclarations || ''}`">
		<img
			v-if="!isEmpty(imageVisibleSourceURI)"
			:src="imageVisibleSourceURI || ''"
			:class="classesImg"
			/>
		<div v-else-if="imageIsValid === false" class="w-full h-full">
			<slot name="fallback"></slot>
		</div>
		<div v-else>
			<!-- Attempting to load before showing fallback. -->
		</div>
	</div>
</template>

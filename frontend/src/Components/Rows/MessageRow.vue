<script setup lang="ts">
import { computed } from 'vue';
import MDIAlert from '@/Components/SVG/Icons/MDIAlert.vue';
import MDICheck from '@/Components/SVG/Icons/MDICheck.vue';
import ImageWithFallback from '@/Components/ImageWithFallback.vue';
import MDIAccount from '@/Components/SVG/Icons/MDIAccount.vue';
import MessageAttachment from '@/Components/Rows/MessageAttachment.vue';
import { useTitle } from '@/_Compostables/GraphQL/Messages/useTitle';
import { useFrom } from '@/_Compostables/GraphQL/Messages/useFrom';
import { useTimestampFormatted } from '@/_Compostables/GraphQL/Messages/useTimestampFormatted';
import { useIsSentByMe } from '@/_Compostables/GraphQL/Messages/useIsSentByMe';
import { useIsSpam } from '@/_Compostables/GraphQL/Messages/useIsSpam';
import { useVCard } from '@/_Compostables/GraphQL/Messages/useVCard';
import { useVCardPhotoURI } from '@/_Compostables/GraphQL/CardDavContacts/useVCardPhotoURI';
import ChatTriangle from '@/Components/SVG/Images/ChatTriangle.vue';
import { useBody } from '@/_Compostables/GraphQL/Messages/useBody';
import { useDataDetectToHTML } from '@/_Compostables/Utility/useDataDetectToHTML';
import { useSanitizeDangerousHTML } from '@/_Compostables/Utility/useSanitizeDangerousHTML';
import { useModelValue } from '@/_Compostables/Utility/useModelValue';
import { useAttachments } from '@/_Compostables/GraphQL/Messages/useAttachments';
import type { ISMSMessage } from '@/_Compostables/GraphQL/Messages/ISMSMessage';

const props = defineProps<{
	modelValue?: ISMSMessage,
	value?: ISMSMessage,
}>();

const emit = defineEmits<{
	(e: 'update:modelValue', payload: ISMSMessage | null): void,
	(e: 'on-value-changed', payload: ISMSMessage | null): void,
}>();

const model = useModelValue(emit, props, null as ISMSMessage | null);

const title = useTitle(model, {
	includeTo: true,
});
const name = useFrom(model);
const timestampFormatted = useTimestampFormatted(model, {
	appearance: 'datetime-short',
});
const isSentByMe = useIsSentByMe(model);
const bodyRaw = useBody(model);
const bodyDataDetect = useDataDetectToHTML(bodyRaw);
const bodySanitized = useSanitizeDangerousHTML(bodyDataDetect);
const isSpam = useIsSpam(model)
const vcard = useVCard(model);
const avatarURI = useVCardPhotoURI(vcard);
const attachments = useAttachments(model);

const bubbleClasses = computed(() => {
	const ret: Record<string, true> = {};
	ret['flex-1 flex flex-col flex-no-wrap gap-0.5'] = true;
	ret['bg-amber-7'] = true;
	ret['px-2 py-1'] = true;
	
	if (isSentByMe.value) {
		ret['rounded-t rounded-bl '] = true;
	} else {
		ret['rounded-t rounded-br '] = true;
	}
	
	return ret;
});




defineExpose({
	title,
	name,
	timestampFormatted,
	isSentByMe,
	isSpam,
	vcard,
	avatarURI,
});

// console.log(props);
</script>
<template>
	<div class="p-2 w-full">
		
		<div class="flex flex-row flex-nowrap">
			
			<div v-if="isSentByMe" class="flex-1"></div>
			
			<!-- Recieved Avatar -->
			<div v-if="!isSentByMe" class="flex flex-col flex-nowrap justify-end">
				<ImageWithFallback
					:src="avatarURI"
					class="w-12 h-12"
					rounded="xl"
					>
					<template #fallback>
						<div class="h-full bg-navy-900 text-navy-0 flex items-center justify-center rounded-xl">
							<MDIAccount class="w-10 h-10" />
						</div>
					</template>
				</ImageWithFallback>
			</div>
			
			<!-- Bubble -->
			<div class="flex flex-row flex-nowrap">
				
				<!-- Triangle thing -->
				<div v-if="!isSentByMe" class="flex flex-row flex-nowrap items-end">
					<ChatTriangle class="w-3 h-3 text-amber-7" />
				</div>
				
				<!-- Main Bubble -->
				<div class="flex flex-col flex-nowrap">
					<div class="text-sm">{{title}}</div>
					<div :class="bubbleClasses">
						
						<!-- Recieved Content -->
						<div v-html="bodySanitized"></div>
						
						<MessageAttachment 
							v-for="(attachment, attachmentIdx) of attachments"
							:key="attachmentIdx"
							:value="attachment"
							/>
						
						<!-- Timestamp & Icons -->
						<div class="flex flex-row gap-x-2 text-xs">
							<div>{{timestampFormatted}}</div>
							<div>
								<abbr class="flex flex-row gap-x-1" style="text-decoration: none;" title="Spam" v-if="isSpam === 'spam'">
									<MDIAlert class="w-3 h-3" />
									Spam
								</abbr>
								<abbr title="Ham" style="text-decoration: none;" v-else-if="isSpam === 'ham'">
									<MDICheck class="w-3 h-3" />
								</abbr>
							</div>
						</div>
					</div>
					
				</div>
				<!-- Triangle thing -->
				<div v-if="isSentByMe" class="flex flex-row flex-nowrap items-end">
					<ChatTriangle class="w-3 h-3 text-amber-7" style="transform: scaleX(-1);" />
				</div>
			</div>
			
			<!-- Sent Avatar -->
			<div v-if="isSentByMe" class="flex flex-col flex-nowrap justify-end">
				<ImageWithFallback
					:src="avatarURI"
					class="w-12 h-12"
					rounded="xl"
					>
					<template #fallback>
						<div class="h-full bg-navy-900 text-navy-0 flex items-center justify-center rounded-xl">
							<MDIAccount class="w-10 h-10" />
						</div>
					</template>
				</ImageWithFallback>
			</div>
			
		</div>
	</div>
</template>
<style scoped>
</style>
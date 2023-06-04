import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import { computed, type Ref } from 'vue';
import type { IQueryResponse } from '../IQueryResponse';
import type { ApolloError } from '@apollo/client/errors';

const useQueryForE164 = (e164: Ref<string | null>) => {

	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { result, onError, loading, onResult, error, refetch } = useQuery<IQueryResponse>(gql`
		query forE164($e164: String!) {
			messages {
				id,
				forE164(e164: $e164) {
					id,
					twilioToCountry,
					twilioToState,
					twilioSmsMessageSid,
					twilioNumMedia,
					twilioToCity,
					twilioFromZip,
					twilioSmsSid,
					twilioFromState,
					twilioSmsStatus,
					twilioFromCity,
					twilioFromCountry,
					twilioMessagingServiceSid,
					twilioToZip,
					twilioMessageSid,
					twilioAccountSid,
					twilioURI,
					twilioApiVersion,
					twilioMediaUrl0,
					twilioMediaUrl1,
					twilioMediaUrl2,
					twilioMediaUrl3,
					twilioMediaUrl4,
					twilioMediaUrl5,
					twilioMediaUrl6,
					twilioMediaUrl7,
					twilioMediaUrl8,
					twilioMediaUrl9,
					iso8601Timestamp,
					price,
					from,
					errorMessage,
					isAIDetectedSpam,
					isHumanConfirmedSpam,
					backendService,
					body,
					to,
					isRead,
					attachments {
						id,
						key,
						mediaType,
						imageWidthPx,
						imageHeightPx,
						uri,
					},
					isSpam,
				},
			},
		}
		`, {
			e164: e164.value,
		}, {
		pollInterval: 1000 * 2,
	});
	
	

	onError((error: ApolloError) => {
		console.error('useQueryAll error', error);
	});

	const prop = computed(() => {
		if (!result.value) {
			return [];
		}
		return result.value?.messages?.forE164 || [];
	});
	
	const reFetch = () => {
		refetch({
			e164: e164.value,
		});
	}

	return {
		forE164: prop,
		forE164Loading: loading,
		forE164OnError: onError,
		forE164Error: error,
		forE164OnResult: onResult,
		forE164ReFetch: reFetch,
	}
}

export { useQueryForE164 }
import { useQuery } from '@vue/apollo-composable'
import gql from 'graphql-tag'
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import { computed } from 'vue';
import type { IQueryResponse } from '../IQueryResponse';
import type { ApolloError } from '@apollo/client/errors';

const useQueryLatestMessages = () => {

	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)

	const { result, onError, loading, onResult, error, refetch } = useQuery<IQueryResponse>(gql`
		query {
			messages {
				id,
				latestMessages {
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
		`, null, {
		pollInterval: 1000,
	});

	onError((error: ApolloError) => {
		console.error('useQueryAll error', error);
	});

	const prop = computed(() => {
		if (!result.value) {
			return [];
		}
		return result.value?.messages?.latestMessages || [];
	});
	
	const reFetch = () => {
		refetch();
	}

	return {
		latest: prop,
		latestLoading: loading,
		latestOnError: onError,
		latestError: error,
		latestOnResult: onResult,
		latestReFetch: reFetch,
	}
}

export { useQueryLatestMessages }